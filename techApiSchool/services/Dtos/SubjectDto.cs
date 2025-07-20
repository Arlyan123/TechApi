/// <summary>
/// cursos DTO
/// </summary>
public class SubjectDto
{
    /// <summary>
    /// Nombre del Curso
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Id del profesor al que le pertence ese curso
    /// </summary>
    public Guid TeacherId { get; set; }

}