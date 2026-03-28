using Core.Common;
using Core.Dto;
using Core.Entities;

namespace Core.Services;

// Interfejs serwisu dla operacji na studentach.
public interface IStudentService
{
    Task<PagedResult<StudentSummaryDto>> FindAllStudentsPaged(int page, int size);
    Task<StudentDetailDto?> GetById(Guid id);
    Task<StudentSummaryDto> AddStudent(StudentCreateDto dto);
    Task<StudentSummaryDto?> UpdateStudent(Guid id, StudentUpdateDto dto);
    Task ChangeStatus(Guid id, StudentStatus status);
}
