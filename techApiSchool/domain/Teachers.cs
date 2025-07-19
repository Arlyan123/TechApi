using System.ComponentModel.DataAnnotations;

namespace domain;

//clase profesores
public class Teachers
{
    #region columnas / informaci�n profesores
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Display(Name = "Nombre Completo")]
    public string? FullName { get; set; } = string.Empty;

    [Display(Name = "Esta Eliminado")]
    public bool IsDeleted { get; set; } = false;

    #endregion


    #region tablas relacionadas 

    // Relaci�n co materias
    public ICollection<Subjects>? Subjects { get; set; } = new List<Subjects>();

    #endregion
}
