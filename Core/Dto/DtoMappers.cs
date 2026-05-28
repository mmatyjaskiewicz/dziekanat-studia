using Core.Entities;
namespace Core.Dto;
public static class DtoMappers
{
    public static StudentSummaryDto ToSummary(this Student student)
    {
        return new StudentSummaryDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            StudentId = student.StudentNumber,
            ProgramName = student.DegreeProgram?.Name ?? string.Empty,
            YearOfStudy = student.YearOfStudy,
            Status = student.Status
        };
    }
    public static StudentDetailDto ToDetail(this Student student)
    {
        return new StudentDetailDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            StudentId = student.StudentNumber,
            ProgramCode = student.DegreeProgram?.Code ?? string.Empty,
            ProgramName = student.DegreeProgram?.Name ?? string.Empty,
            EnrollmentYear = student.EnrollmentYear?.Name ?? string.Empty,
            YearOfStudy = student.YearOfStudy,
            Status = student.Status,
            GradePointAverage = 0,
            TotalEctsEarned = 0,
            IsEligibleForDiploma = false
        };
    }
    public static Student ToEntity(this StudentCreateDto dto)
    {
        return new Student
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            NationalId = dto.NationalId,
            StudentNumber = dto.StudentId,
            YearOfStudy = dto.YearOfStudy,
            Status = StudentStatus.Active
        };
    }
    public static LecturerSummaryDto ToSummary(this Lecturer lecturer)
    {
        return new LecturerSummaryDto
        {
            Id = lecturer.Id,
            FirstName = lecturer.FirstName,
            LastName = lecturer.LastName,
            Email = lecturer.Email,
            Title = lecturer.Title,
            DisplayName = $"{lecturer.Title} {lecturer.FirstName} {lecturer.LastName}".Trim()
        };
    }
    public static LecturerDetailDto ToDetail(this Lecturer lecturer)
    {
        return new LecturerDetailDto
        {
            Id = lecturer.Id,
            FirstName = lecturer.FirstName,
            LastName = lecturer.LastName,
            Email = lecturer.Email,
            Title = lecturer.Title,
            Faculty = lecturer.Faculty
        };
    }
    public static Lecturer ToEntity(this LecturerCreateDto dto)
    {
        return new Lecturer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            NationalId = dto.NationalId,
            Title = dto.Title,
            Faculty = dto.Faculty
        };
    }
}

