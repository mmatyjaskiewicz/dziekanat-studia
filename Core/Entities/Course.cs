namespace Core.Entities;
public class Course : EntityBase
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Ects { get; set; }
}

