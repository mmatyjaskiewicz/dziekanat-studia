using Core.Common;
using Core.Dto;
using Core.Entities;
using Core.Exceptions;
using Core.Repositories;
using Core.UnitOfWork;
namespace Core.Services;
public class StudentService(IUniversityUnitOfWork unitOfWork) : IStudentService
{
    public async Task<PagedResult<StudentSummaryDto>> FindAllStudentsPaged(int page, int size)
    {
        var people = await unitOfWork.Students.FindPagedAsync(page, size);
        var items = people.Items.Select(p => p.ToSummary()).ToList();
        return new PagedResult<StudentSummaryDto>(items, people.TotalCount, people.Page, people.PageSize);
    }
    public async Task<StudentDetailDto?> GetById(Guid id)
    {
        var student = await unitOfWork.Students.FindByIdAsync(id);
        return student?.ToDetail();
    }
    public async Task<StudentSummaryDto> AddStudent(StudentCreateDto dto)
    {
        var entity = dto.ToEntity();
        entity = await unitOfWork.Students.AddAsync(entity);
        await unitOfWork.SaveChangesAsync();
        return entity.ToSummary();
    }
    public async Task<StudentSummaryDto?> UpdateStudent(Guid id, StudentUpdateDto dto)
    {
        var student = await unitOfWork.Students.FindByIdAsync(id);
        if (student is null) return null;
        student.FirstName = dto.FirstName;
        student.LastName = dto.LastName;
        student.Email = dto.Email;
        student.YearOfStudy = dto.YearOfStudy;
        student.Status = dto.Status;
        student = await unitOfWork.Students.UpdateAsync(student);
        await unitOfWork.SaveChangesAsync();
        return student.ToSummary();
    }
    public async Task ChangeStatus(Guid id, StudentStatus status)
    {
        await unitOfWork.Students.ChangeStatusAsync(id, status);
        await unitOfWork.SaveChangesAsync();
    }
    public async Task<GradeDto> AddGrade(Guid studentId, GradeCreateDto gradeDto, string changedBy)
    {
        var student = await unitOfWork.Students.FindByIdAsync(studentId)
            ?? throw new StudentNotFoundException($"Student with id={studentId} not found!");
        var course = await unitOfWork.Courses.FindByIdAsync(gradeDto.CourseId)
            ?? throw new Exception($"Course with id={gradeDto.CourseId} not found!");
        var lecturer = await unitOfWork.Lecturers.FindByIdAsync(gradeDto.LecturerId)
            ?? throw new LecturerNotFoundException($"Lecturer with id={gradeDto.LecturerId} not found!");
        var year = await unitOfWork.AcademicYears.FindByIdAsync(gradeDto.AcademicYearId)
            ?? throw new Exception($"Academic year with id={gradeDto.AcademicYearId} not found!");

        var grade = new Grade
        {
            StudentId = student.Id,
            CourseId = course.Id,
            LecturerId = lecturer.Id,
            AcademicYearId = year.Id,
            IssueDate = gradeDto.IssueDate,
            Value = gradeDto.GradeValue
        };

        grade = await unitOfWork.Grades.AddAsync(grade);
        await unitOfWork.SaveChangesAsync();

        return new GradeDto
        {
            Id = grade.Id,
            CourseId = grade.CourseId,
            LecturerId = grade.LecturerId,
            AcademicYearId = grade.AcademicYearId,
            IssueDate = grade.IssueDate,
            GradeValue = grade.Value
        };
    }
    public async Task<IEnumerable<GradeDto>> GetGrades(Guid studentId)
    {
        var student = await unitOfWork.Students.FindByIdAsync(studentId)
            ?? throw new StudentNotFoundException($"Student with id={studentId} not found!");
        return student.Grades.Select(g => new GradeDto
        {
            Id = g.Id,
            CourseId = g.CourseId,
            LecturerId = g.LecturerId,
            AcademicYearId = g.AcademicYearId,
            IssueDate = g.IssueDate,
            GradeValue = g.Value
        }).ToList();
    }
    public async Task<GradeDto?> UpdateGrade(Guid studentId, Guid gradeId, GradeUpdateDto dto, string changedBy)
    {
        var student = await unitOfWork.Students.FindByIdAsync(studentId)
            ?? throw new StudentNotFoundException($"Student with id={studentId} not found!");
        var grade = student.Grades.FirstOrDefault(g => g.Id == gradeId);
        if (grade is null) return null;

        var oldValue = grade.Value;
        var oldDate = grade.IssueDate;
        grade.IssueDate = dto.IssueDate;
        grade.Value = dto.GradeValue;

        return new GradeDto
        {
            Id = grade.Id,
            CourseId = grade.CourseId,
            LecturerId = grade.LecturerId,
            AcademicYearId = grade.AcademicYearId,
            IssueDate = grade.IssueDate,
            GradeValue = grade.Value
        };
    }
    public Task<IEnumerable<StudentSummaryDto>> GetStudentsByLecturer(Guid lecturerId)
    {
        var byYear = unitOfWork.Students.GetStudentsByStudyYear(0);
        var all = byYear.Where(s => s.Grades.Any(g => g.LecturerId == lecturerId)).ToList();
        return Task.FromResult<IEnumerable<StudentSummaryDto>>(all.Select(s => s.ToSummary()).ToList());
    }
    public async Task AssignToProgram(Guid studentId, Guid degreeProgramId, Guid? academicYearId)
    {
        var student = await unitOfWork.Students.FindByIdAsync(studentId)
            ?? throw new StudentNotFoundException($"Student with id={studentId} not found!");
        student.DegreeProgramId = degreeProgramId;
        if (academicYearId is not null)
            student.EnrollmentYearId = academicYearId;
        await unitOfWork.Students.UpdateAsync(student);
        await unitOfWork.SaveChangesAsync();
    }
    public async Task ChangeStatus(Guid studentId, StudentStatus status, string reason)
    {
        await ChangeStatus(studentId, status);
    }
    public Task<IEnumerable<GradeChangeHistoryDto>> GetGradeHistory(Guid gradeId)
    {
        return Task.FromResult<IEnumerable<GradeChangeHistoryDto>>(Array.Empty<GradeChangeHistoryDto>());
    }
}
