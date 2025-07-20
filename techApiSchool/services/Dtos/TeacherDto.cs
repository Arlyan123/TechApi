/// <summary>
/// DTO del profe
/// </summary>
public class TeacherDto
{
    /// <summary>
    /// Nombre completo del profe y yap
    /// </summary>
    public string FullName { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
}

public class TeacherDtoC
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public List<string>? Subjects { get; set; }
}
