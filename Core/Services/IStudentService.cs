using Core.Common;
using Core.Dto;
using Core.Entities;
namespace Core.Services;
public interface IStudentService
{
    Task<PagedResult<StudentSummaryDto>> FindAllStudentsPaged(int page, int size);
    Task<StudentDetailDto?> GetById(Guid id);
    Task<StudentSummaryDto> AddStudent(StudentCreateDto dto);
    Task<StudentSummaryDto?> UpdateStudent(Guid id, StudentUpdateDto dto);
    Task ChangeStatus(Guid id, StudentStatus status);
    Task<GradeDto> AddGrade(Guid studentId, GradeCreateDto gradeDto, string changedBy);
    Task<IEnumerable<GradeDto>> GetGrades(Guid studentId);
    Task<GradeDto?> UpdateGrade(Guid studentId, Guid gradeId, GradeUpdateDto dto, string changedBy);
    Task<IEnumerable<StudentSummaryDto>> GetStudentsByLecturer(Guid lecturerId);
    Task AssignToProgram(Guid studentId, Guid degreeProgramId, Guid? academicYearId);
    Task ChangeStatus(Guid studentId, StudentStatus status, string reason);
    Task<IEnumerable<GradeChangeHistoryDto>> GetGradeHistory(Guid gradeId);
}
