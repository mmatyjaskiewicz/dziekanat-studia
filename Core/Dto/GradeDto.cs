using Core.Entities;
namespace Core.Dto;
public class GradeCreateDto
{
    public Guid CourseId { get; init; }
    public Guid LecturerId { get; init; }
    public Guid AcademicYearId { get; init; }
    public DateTime IssueDate { get; init; } = DateTime.UtcNow;
    public GradeValue GradeValue { get; init; }
}
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
public class GradeUpdateDto
{
    public DateTime IssueDate { get; init; }
    public GradeValue GradeValue { get; init; }
}

