using System.ComponentModel.DataAnnotations;

namespace domain;

//clase cursos 
public class Subjects
{
    #region columnas / información de los cursos
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Display(Name = "Nombre Curso")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Id Profesor")]
    public Guid TeacherId { get; set; }

    [Display(Name = "Esta Eliminado")]
    public bool IsDeleted { get; set; } = false;


    #endregion

    #region tablas relacionadas 

    public Teachers? Teacher { get; set; }

    // Relación calificaciones
    public ICollection<StudentGrades>? StudentGrades { get; set; } = new List<StudentGrades>();

    #endregion
}
