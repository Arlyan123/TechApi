using domain;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace services;

public class StudentGradesService
{
    private readonly AppDbContext _db;
    public StudentGradesService(AppDbContext db) => _db = db;

 
    public async Task<List<StudentGradeDtoC>> GetAllAsync()
    {
        return await _db.StudentGrades
            .Include(g => g.Student)
            .Include(g => g.Subject)
            .Select(g => new StudentGradeDtoC
            {
                Id = g.Id,
                GradeValue = g.GradeValue,
                StudentId = g.StudentId,
                StudentName = g.Student.Name,
                SubjectId = g.SubjectId,
                SubjectName = g.Subject.Name
            })
            .ToListAsync();
    }


    public async Task<StudentGrades?> GetByIdAsync(Guid id) =>
        await _db.StudentGrades
            .FirstOrDefaultAsync(g => g.Id == id);


    public async Task CreateAsync(StudentGrades dto)
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

    public async Task<bool> UpdateAsync(Guid id, StudentGradeDtoAU dto)
    {
        var grade = await _db.StudentGrades.FindAsync(id);
        if (grade == null)
            return false;

        grade.StudentId = dto.StudentId;
        grade.SubjectId = dto.SubjectId;
        grade.GradeValue = dto.GradeValue;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task DeleteAsync(Guid id)
    {
        var grade = await _db.StudentGrades.FindAsync(id);
        if (grade == null) throw new KeyNotFoundException("Calificación no encontrada");

        grade.IsDeleted = true;
        _db.StudentGrades.Update(grade);
        await _db.SaveChangesAsync();
    }


}
