using Core.Common;
using Core.Dto;
using Core.Entities;
using Core.Services;
using Core.UnitOfWork;

namespace Infrastructre.Memory;

// Implementacja serwisu Student korzystająca z jednostki pracy w pamięci.
public class MemoryStudentService(IUniversityUnitOfWork unitOfWork) : IStudentService
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
}
