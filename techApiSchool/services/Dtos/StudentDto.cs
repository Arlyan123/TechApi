/// <summary>
/// DTO del estudiante para que no haya necesidad de escribir el id
/// </summary>
public class StudentDto
{
    /// <summary>
    /// Nombre del Estudiante
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Edad del Estudiante
    /// </summary>
    public int Age { get; set; }
    /// <summary>
    /// Correo del Estudiante
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
