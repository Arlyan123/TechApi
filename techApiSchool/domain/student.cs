namespace domain;

public class student
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string Email { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
