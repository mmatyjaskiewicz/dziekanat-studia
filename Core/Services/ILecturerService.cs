using Core.Common;
using Core.Dto;
using Core.Entities;

namespace Core.Services;

public interface ILecturerService
{
    Task<PagedResult<LecturerSummaryDto>> GetAllLecturersPaged(int page, int size);
    Task<LecturerDetailDto?> GetLecturerById(Guid id);
    Task<LecturerSummaryDto> AddLecturer(LecturerCreateDto dto);
    Task<LecturerSummaryDto?> UpdateLecturer(Guid id, LecturerUpdateDto dto);
    Task<IEnumerable<CourseDto>> GetLecturerCourses(Guid lecturerId);
}
