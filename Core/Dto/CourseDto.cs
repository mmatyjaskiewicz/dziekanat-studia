namespace Core.Dto;

public class CourseDto
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public int Ects { get; init; }
}
