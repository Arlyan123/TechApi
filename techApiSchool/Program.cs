using infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using services;
using System;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbschool")));


/// <summary>
/// Servicios de las tablas del CRUD
/// </summary>

builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<jwtt>();
builder.Services.AddScoped<TeacherService, TeacherService>();
builder.Services.AddScoped<SubjectsService, SubjectsService>();
builder.Services.AddScoped<StudentGradesService, StudentGradesService>();

var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
        };
    });




builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();


/// <summary>
/// Authorization con token
/// </summary>
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Student API", Version = "v1" });

    c.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
    {
        Description = "Ingresa Bearer + token de login",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "JWT"
            }
        },
        Array.Empty<string>()
    }
    });

});

/// <summary>
/// Documentación
/// </summary>
builder.Services.AddSwaggerGen(c =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


builder.Services.AddControllers();
var app = builder.Build();

/// <summary>
/// Usar Swagger
/// </summary>
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
