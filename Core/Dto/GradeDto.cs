using Core.Entities;

namespace Core.Dto;

// DTO pojedynczej oceny - przekazywane przy dodawaniu/edycji.
public class GradeCreateDto
{
    public Guid CourseId { get; init; }
    public Guid LecturerId { get; init; }
    public Guid AcademicYearId { get; init; }
    public DateTime IssueDate { get; init; } = DateTime.UtcNow;
    public GradeValue GradeValue { get; init; }
}

// DTO zwracane po dodaniu oceny.
public class GradeDto
{
    public Guid Id { get; init; }
    public Guid CourseId { get; init; }
    public Guid LecturerId { get; init; }
    public Guid AcademicYearId { get; init; }
    public DateTime IssueDate { get; init; }
    public GradeValue GradeValue { get; init; }
    public double NumericValue => GradeValue.Value();
}

// DTO edycji istniejącej oceny.
public class GradeUpdateDto
{
    public DateTime IssueDate { get; init; }
    public GradeValue GradeValue { get; init; }
}
