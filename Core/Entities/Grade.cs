namespace Core.Entities;
public class Grade : EntityBase
{
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    public Guid LecturerId { get; set; }
    public Guid AcademicYearId { get; set; }
    public DateTime IssueDate { get; set; }
    public GradeValue Value { get; set; }
}

