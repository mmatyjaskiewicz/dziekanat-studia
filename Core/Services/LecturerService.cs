using Core.Common;
using Core.Dto;
using Core.Entities;
using Core.UnitOfWork;

namespace Core.Services;

public class LecturerService(IUniversityUnitOfWork unitOfWork) : ILecturerService
{
    public async Task<PagedResult<LecturerSummaryDto>> GetAllLecturersPaged(int page, int size)
    {
        var all = await unitOfWork.Lecturers.FindPagedAsync(page, size);
        var items = all.Items.Select(l => l.ToSummary()).ToList();
        return new PagedResult<LecturerSummaryDto>(items, all.TotalCount, all.Page, all.PageSize);
    }

    public async Task<LecturerDetailDto?> GetLecturerById(Guid id)
    {
        var l = await unitOfWork.Lecturers.FindByIdAsync(id);
        return l?.ToDetail();
    }

    public async Task<LecturerSummaryDto> AddLecturer(LecturerCreateDto dto)
    {
        var entity = dto.ToEntity();
        entity = await unitOfWork.Lecturers.AddAsync(entity);
        await unitOfWork.SaveChangesAsync();
        return entity.ToSummary();
    }

    public async Task<LecturerSummaryDto?> UpdateLecturer(Guid id, LecturerUpdateDto dto)
    {
        var l = await unitOfWork.Lecturers.FindByIdAsync(id);
        if (l is null) return null;

        l.FirstName = dto.FirstName;
        l.LastName = dto.LastName;
        l.Email = dto.Email;
        l.Title = dto.Title;
        l.Faculty = dto.Faculty;

        l = await unitOfWork.Lecturers.UpdateAsync(l);
        await unitOfWork.SaveChangesAsync();
        return l.ToSummary();
    }

    public async Task<IEnumerable<CourseDto>> GetLecturerCourses(Guid lecturerId)
    {
        var lecturer = await unitOfWork.Lecturers.FindByIdAsync(lecturerId);
        if (lecturer is null) return Array.Empty<CourseDto>();

        return lecturer.Courses.Select(c => new CourseDto
        {
            Id = c.Id,
            Code = c.Code,
            Name = c.Name,
            Ects = c.Ects
        }).ToList();
    }
}
