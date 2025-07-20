/// <summary>
/// DTO de las calificaciones para la Consulta
/// </summary>
public class StudentGradeDtoC
{
    /// <summary>
    /// Id de la calificaci�n
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Valor de la calificaci�n
    /// </summary>
    public decimal? GradeValue { get; set; }
    /// <summary>
    /// Id del estudiante
    /// </summary>
    public Guid? StudentId { get; set; }
    /// <summary>
    /// Nombre del estudiante
    /// </summary>
    public string? StudentName { get; set; } = string.Empty;
    /// <summary>
    /// Id del Curso o subject
    /// </summary>
    public Guid? SubjectId { get; set; }
    /// <summary>
    /// Nombre del Curso o subject
    /// </summary>
    public string? SubjectName { get; set; } = string.Empty;
}
/// <summary>
/// DTO de las calificaciones para la Creaci�n y Actualizaci�n
/// </summary>
public class StudentGradeDtoAU
{
    /// <summary>
    /// Valor de la calificaci�n 
    /// </summary>
    public decimal? GradeValue { get; set; }
    /// <summary>
    /// Id del estudiante
    /// </summary>
    public Guid? StudentId { get; set; }
    /// <summary>
    /// Id de Curso
    /// </summary>
    public Guid? SubjectId { get; set; }
}
