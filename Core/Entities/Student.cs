using Core.Entities;

namespace Core.Entities;

// Klasa studenta - dziedziczy po klasie bazowej Person.
public class Student : Person
{
    public string StudentNumber { get; set; } = string.Empty;
    public int YearOfStudy { get; set; } = 1;
    public Guid? DegreeProgramId { get; set; }
    public DegreeProgram? DegreeProgram { get; set; }
    public Guid? EnrollmentYearId { get; set; }
    public AcademicYear? EnrollmentYear { get; set; }
    public StudentStatus Status { get; set; } = StudentStatus.Active;
    public List<Grade> Grades { get; set; } = new();
}
