using System.ComponentModel.DataAnnotations;

namespace domain;

//clase estudiante
public class Student
{
    #region columnas / informaci�n estudiante
    [Key]
    public Guid Id 
    { get; set; } = Guid.NewGuid();

    //display en espa�ol para ser usado en auditoria
    [Display(Name = "Nombre")]
    public string? Name 
    { get; set; } = null!;

    [Display(Name = "Edad")]
    public int? Age 
    { get; set; }

    [Display(Name = "Correo")]
    public string? Email 
    { get; set; } = null!;

    [Display(Name = "Fecha Creaci�n")]
    public DateTime? CreatedAt 
    { get; set; } = DateTime.UtcNow;

    [Display(Name = "Esta Eliminado")]
    public bool IsDeleted { get; set; } = false;
    #endregion


    #region tablas relacionadas 

    public ICollection<StudentGrades> StudentGrades { get; set; } = new List<StudentGrades>();

    #endregion
}
