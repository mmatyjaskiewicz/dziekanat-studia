using Core.Entities;

namespace Core.Dto;

// DTO skrótowego opisu studenta - używane np. na listach.
public sealed record StudentSummaryDto : PersonDto
{
    public string StudentId { get; init; } = string.Empty;
    public string ProgramName { get; init; } = string.Empty;
    public int YearOfStudy { get; init; }
    public StudentStatus Status { get; init; }
}

// DTO pełnych danych studenta - zwracane np. po pobraniu po Id.
public sealed record StudentDetailDto : PersonDto
{
    public string StudentId { get; init; } = string.Empty;
    public string ProgramCode { get; init; } = string.Empty;
    public string ProgramName { get; init; } = string.Empty;
    public string EnrollmentYear { get; init; } = string.Empty;
    public int YearOfStudy { get; init; }
    public StudentStatus Status { get; init; }
    public double GradePointAverage { get; init; }
    public int TotalEctsEarned { get; init; }
    public bool IsEligibleForDiploma { get; init; }
}

// DTO tworzenia nowego studenta.
public sealed record StudentCreateDto : PersonCreateDto
{
    public string StudentId { get; init; } = string.Empty;
    public int YearOfStudy { get; init; } = 1;
    public string ProgramCode { get; init; } = string.Empty;
    public int EnrollmentYearFrom { get; init; }
}

// DTO aktualizacji danych studenta.
public sealed record StudentUpdateDto : PersonDto
{
    public int YearOfStudy { get; init; }
    public StudentStatus Status { get; init; }
    public string ProgramCode { get; init; } = string.Empty;
}
