namespace Core.Entities;

// Klasa oceny przypisanej do studenta z konkretnego kursu, w danym roku akademickim.
public class Grade : EntityBase
{
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    public Guid LecturerId { get; set; }
    public Guid AcademicYearId { get; set; }
    public DateTime IssueDate { get; set; }
    public GradeValue Value { get; set; }
}
