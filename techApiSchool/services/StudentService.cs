using domain;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace services;

/// <summary>
/// Servicio para estudiantes
/// </summary>

public class StudentService
{
    private readonly AppDbContext _db;
    public StudentService(AppDbContext db)
    {
        _db = db;
    }

    //get all omite los "eliminados"
    public async Task<List<Student>> GetAllAsync()
    {
        return await _db.Students
            .Where(s => !s.IsDeleted)
            .ToListAsync();
    }

    public Task<Student?> GetByIdAsync(Guid id) => _db.Students.FindAsync(id).AsTask();

    public async Task CreateAsync(Student student)
    {
        await _db.Students.AddAsync(student);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(Guid id, StudentDto dto)
    {
        var existing = await _db.Students.FindAsync(id);
        if (existing == null)
            return false;

        existing.Name = dto.Name;
        existing.Age = dto.Age;
        existing.Email = dto.Email;

        await _db.SaveChangesAsync();
        return true;
    }

    //Delete cambia de estado para no eliminarlo completamente
    public async Task<bool> DeleteAsync(Guid id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student != null)
        {
            student.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }

        return false;
    }

}
