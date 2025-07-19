using System.ComponentModel.DataAnnotations;

namespace domain;

//clase calificaciones 
public class StudentGrades
{
    #region columnas / informaci�n calificaciones
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Display(Name = "Estudiante")]
    public Guid? StudentId { get; set; }

    [Display(Name = "Curso")]
    public Guid? SubjectId { get; set; }

    [Display(Name = "Calificaci�n")]
    public decimal? GradeValue { get; set; }

    [Display(Name = "Fecha Calificaci�n")]
    public DateTime? GradedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "Esta Eliminado")]
    public bool IsDeleted { get; set; } = false;

    #endregion

    //cursos y estudiantes
    #region tablas relacionadas 
    public Subjects? Subject { get; set; }
    public Student? Student { get; set; }

    #endregion
}
