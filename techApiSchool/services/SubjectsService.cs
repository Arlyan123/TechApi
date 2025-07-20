using domain;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace services;

public class SubjectsService
{
    private readonly AppDbContext _db;
    public SubjectsService(AppDbContext db) => _db = db;

    //get all omite los "eliminados"
    public async Task<List<Subjects>> GetAllAsync()
    {
        return await _db.Subjects
            .Where(s => !s.IsDeleted)
            .ToListAsync();
    }

    public Task<Subjects?> GetByIdAsync(Guid id) => _db.Subjects.FindAsync(id).AsTask();

    public async Task<Guid> CreateAsync(SubjectDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Nombre del Curso es requerido.");

        var teacher = await _db.Teachers.FindAsync(dto.TeacherId);
        if (teacher == null)
            throw new KeyNotFoundException("Profesor no encontrado.");

        var subject = new Subjects
        {
            Name = dto.Name,
            TeacherId = dto.TeacherId
        };

        _db.Subjects.Add(subject);
        await _db.SaveChangesAsync();

        return teacher.Id;
    }

    public async Task UpdateAsync(Guid id, SubjectDto dto)
    {
        var subject = await _db.Subjects.FindAsync(id);
        if (subject == null) throw new KeyNotFoundException("Profesor no encontrado");

        subject.Name = dto.Name;
        subject.TeacherId = dto.TeacherId;
        await _db.SaveChangesAsync();
    }

    //Delete cambia de estado para no eliminarlo completamente
    public async Task<bool> DeleteAsync(Guid id)
    {
        var Subjects = await _db.Subjects.FindAsync(id);
        if (Subjects != null)
        {
            Subjects.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }

        return false;
    }

}
