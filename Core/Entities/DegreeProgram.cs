namespace Core.Entities;
public class DegreeProgram : EntityBase
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int DurationYears { get; set; } = 3;
}

