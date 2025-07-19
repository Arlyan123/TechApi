using domain;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace service;

public class StudentService
{
    private readonly AppDbContext _db;
    public StudentService(AppDbContext db) => _db = db;

    public Task<List<student>> GetAllAsync() => _db.Students.ToListAsync();

    public Task<student?> GetByIdAsync(Guid id) => _db.Students.FindAsync(id).AsTask();

    public async Task CreateAsync(student student)
    {
        await _db.Students.AddAsync(student);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(student student)
    {
        _db.Students.Update(student);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student != null)
        {
            _db.Students.Remove(student);
            await _db.SaveChangesAsync();
        }
    }
}
