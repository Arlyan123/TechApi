using domain;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace services;

/// <summary>
/// Servicio para Profesores
/// </summary> 
public class TeacherService
{
    private readonly AppDbContext _db;

    public TeacherService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Teachers>> GetAllAsynca()
        => await _db.Teachers.Where(t => !t.IsDeleted)
        .ToListAsync();


    public async Task<List<TeacherDtoC>> GetAllAsync()
    {
        return await _db.Teachers
            .Where(t => !t.IsDeleted)
            .Include(t => t.Subjects)
            .Select(t => new TeacherDtoC
            {
                Id = t.Id,
                FullName = t.FullName,
                Telefono = t.Telefono,
                Direccion = t.Direccion,
                Subjects = t.Subjects.Select(s => s.Name).ToList()
            })
            .ToListAsync();
    }

    public async Task<Teachers?> GetByIdAsync(Guid id) => await _db.Teachers.Include(t => t.Subjects).FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

    public async Task<Guid> CreateAsync(TeacherDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
            throw new ArgumentException("FullName es requerido!");

        var teacher = new Teachers { FullName = dto.FullName , Direccion = dto.Direccion, Telefono = dto.Telefono};
        _db.Teachers.Add(teacher);
        await _db.SaveChangesAsync();

        return teacher.Id;
    }


    public async Task UpdateAsync(Guid id, TeacherDto dto)
    {
        var teacher = await _db.Teachers.FindAsync(id);
        if (teacher == null) throw new KeyNotFoundException("Profesor no encontrado!");

        if (string.IsNullOrWhiteSpace(dto.FullName))
            throw new ArgumentException("Fullname es requerido!");

        teacher.FullName = dto.FullName;
        teacher.Telefono = dto.Telefono;
        teacher.Direccion = dto.Direccion;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var teacher = await _db.Teachers.Include(t => t.Subjects).FirstOrDefaultAsync(t => t.Id == id);
        if (teacher == null) throw new KeyNotFoundException("Profesor no encontrado!");

        if (teacher.Subjects.Any())
            throw new InvalidOperationException("No puede eliminar un profesor con cursos asignados!");

        teacher.IsDeleted = true;
        await _db.SaveChangesAsync();
    }

}
