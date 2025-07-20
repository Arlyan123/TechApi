using domain;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.AccessControl;
using System.Text.Json;

namespace infrastructure;

public class AppDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor)
    : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public DbSet<Student> Students => Set<Student>();
    public DbSet<Audit> Audit => Set<Audit>();
    public DbSet<Teachers> Teachers { get; set; }
    public DbSet<Subjects> Subjects { get; set; }
    public DbSet<StudentGrades> StudentGrades { get; set; }

    #region convertir las clases a tablas en la db
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //excluir registros en todas partes que tengan IsDeleted en verdadero
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var isDeletedProp = entityType.ClrType.GetProperty("IsDeleted");
            if (isDeletedProp != null && isDeletedProp.PropertyType == typeof(bool))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Call(
                    typeof(EF), nameof(EF.Property), new[] { typeof(bool) },
                    parameter, Expression.Constant("IsDeleted")
                );
                var compare = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(compare, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }

        modelBuilder.Entity<Student>().ToTable("students");
        modelBuilder.Entity<Audit>().ToTable("audit");
        modelBuilder.Entity<Teachers>().ToTable("Teachers");
        modelBuilder.Entity<Subjects>().ToTable("Subjects");
        modelBuilder.Entity<StudentGrades>().ToTable("StudentGrades");
    }
    #endregion

    #region guardar datos en auditoria
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var user = _httpContextAccessor.HttpContext?.User?.Identity?.Name?? "Unknown";

        var auditLogs = new List<Audit>();

        foreach (var entry in ChangeTracker.Entries().Where(e =>
            e.State == EntityState.Modified || e.State == EntityState.Added || e.State == EntityState.Deleted))
        {
            var entityType = entry.Entity.GetType();
            var tableName = entityType.Name;
            var recordId = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString();

            var infoDictionary = new Dictionary<string, object?>();

            foreach (var prop in entry.Properties)
            {
                var field = prop.Metadata.Name;
                var displayName = GetDisplayName(entityType, field);
                var value = entry.State switch
                {
                    EntityState.Modified => prop.CurrentValue,
                    EntityState.Added => prop.CurrentValue,
                    EntityState.Deleted => prop.OriginalValue,
                    _ => null
                };

                var readableValue = await GetReadableValueAsync(entityType, field, value);
                infoDictionary[displayName ?? field] = readableValue;
            }

            var newValues = JsonSerializer.Serialize(infoDictionary);

            auditLogs.Add(new Audit
            {
                TableName = tableName,
                RecordId = recordId ?? "",
                Action = entry.State.ToString(),
                ModifiedBy = user,
                ModifiedAt = DateTime.UtcNow,
                Infovalue = newValues
            });
        }

        if (auditLogs.Any())
            Audit.AddRange(auditLogs);

        return await base.SaveChangesAsync(cancellationToken);
    }


    #endregion

    #region traer el nombre en español del display
    private static string GetDisplayName(Type type, string propertyName)
    {
        var prop = type.GetProperty(propertyName);
        return prop?.GetCustomAttribute<DisplayAttribute>()?.Name ?? propertyName;
    }

    #endregion

    #region traer la info de la relación
    private async Task<string?> GetReadableValueAsync(Type entityType, string propName, object? value)
    {
        if (value is Guid guidValue && guidValue != Guid.Empty)
        {
            var navName = propName.EndsWith("Id") ? propName[..^2] : null;
            if (navName == null) return guidValue.ToString();

            var navProp = entityType.GetProperty(navName);
            if (navProp == null) return guidValue.ToString();

            var navType = navProp.PropertyType;

            var dbSetProp = this.GetType().GetProperties()
                .FirstOrDefault(p =>
                    p.PropertyType.IsGenericType &&
                    p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                    p.PropertyType.GenericTypeArguments[0] == navType);

            if (dbSetProp?.GetValue(this) is IQueryable<object> query)
            {
                var parameter = Expression.Parameter(navType, "x");
                var idProp = navType.GetProperty("Id");
                if (idProp == null) return guidValue.ToString();

                var left = Expression.Property(parameter, idProp);
                var right = Expression.Constant(guidValue);
                var equal = Expression.Equal(left, right);
                var lambda = Expression.Lambda(equal, parameter);

                var method = typeof(Queryable).GetMethods()
                    .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(navType);

                var filteredQuery = method.Invoke(null, new object[] { query, lambda });

                var resultList = await ((IQueryable<object>)filteredQuery!).ToListAsync();
                var item = resultList.FirstOrDefault();

                if (item != null)
                {
                    var nameProp = item.GetType().GetProperty("Name") ??
                                   item.GetType().GetProperty("FullName") ??
                                   item.GetType().GetProperty("Title");

                    return nameProp?.GetValue(item)?.ToString() ?? guidValue.ToString();
                }
            }
        }

        return value?.ToString();
    }

    #endregion



}
