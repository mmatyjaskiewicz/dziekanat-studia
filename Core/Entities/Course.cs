namespace Core.Entities;

// Klasa reprezentująca kurs prowadzony na uczelni.
public class Course : EntityBase
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Ects { get; set; }
}
