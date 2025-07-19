using domain;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace services;

public class StudentGradesService
{
    private readonly AppDbContext _db;
    public StudentGradesService(AppDbContext db) => _db = db;

    //get all omite los "eliminados"
    public async Task<List<StudentGrades>> GetAllAsync() =>
         await _db.StudentGrades
             .Include(g => g.Student)
             .Include(g => g.Subject)
             .ToListAsync();

    public async Task<StudentGrades?> GetByIdAsync(Guid id) =>
        await _db.StudentGrades
            .Include(g => g.Student)
            .Include(g => g.Subject)
            .FirstOrDefaultAsync(g => g.Id == id);

    public async Task CreateAsync(StudentGradeDto dto)
    {
        var student = await _db.Students.FindAsync(dto.StudentId);
        var subject = await _db.Subjects.FindAsync(dto.SubjectId);

        if (student == null)
            throw new KeyNotFoundException("Estudiante no encontrado.");

        if (subject == null)
            throw new KeyNotFoundException("Curso no encontrado.");

        var grade = new StudentGrades
        {
            StudentId = dto.StudentId,
            SubjectId = dto.SubjectId,
            GradeValue = dto.GradeValue
        };

        _db.StudentGrades.Add(grade);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, StudentGradeDto dto)
    {
        var grade = await _db.StudentGrades.FindAsync(id);
        if (grade == null) throw new KeyNotFoundException("Calificación no encontrada");

        grade.StudentId = dto.StudentId;
        grade.SubjectId = dto.SubjectId;
        grade.GradeValue = dto.GradeValue;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var grade = await _db.StudentGrades.FindAsync(id);
        if (grade == null) throw new KeyNotFoundException("Calificación no encontrada");

        _db.StudentGrades.Remove(grade);
        await _db.SaveChangesAsync();
    }

}
