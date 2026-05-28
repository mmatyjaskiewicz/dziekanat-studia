namespace Core.Entities;
public class AcademicYear : EntityBase
{
    public string Name { get; set; } = string.Empty;       // np. "2025/2026"
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

