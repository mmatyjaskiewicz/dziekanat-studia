using Core.Entities;
using Core.Repositories;
using Infrastructre.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
namespace Infrastructre.EntityFramework.Repositories;
public class EfStudentRepository(UniversityDbContext context)
    : EfGenericRepository<Student>(context.Students), IStudentRepository
{
    public IEnumerable<Student> GetStudentsByStudyYear(int studyYear)
    {
        return _ = context.Students
            .Where(s => s.YearOfStudy == studyYear)
            .AsNoTracking()
            .ToList();
    }
    public IEnumerable<Student> GetStudentsByProgram(Guid programId)
    {
        return _ = context.Students
            .Where(s => s.DegreeProgramId == programId)
            .AsNoTracking()
            .ToList();
    }
    public async Task ChangeStatusAsync(Guid studentId, StudentStatus status)
    {
        var student = await context.Students.FindAsync(studentId)
            ?? throw new KeyNotFoundException($"Student with id={studentId} not found.");
        student.Status = status;
    }
}

