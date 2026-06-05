using Core.Entities;
using Core.Repositories;
using Infrastructre.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
namespace Infrastructre.EntityFramework.Repositories;
public class EfStudentRepository(UniversityDbContext context)
    : EfGenericRepository<Student>(context.Students), IStudentRepository
{
    public override async Task<Student?> FindByIdAsync(Guid id)
    {
        return await context.Students
            .Include(s => s.Grades)
            .Include(s => s.DegreeProgram)
            .Include(s => s.EnrollmentYear)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
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
    public IEnumerable<Student> GetStudentsWithGradeByLecturer(Guid lecturerId)
    {
        var studentIds = context.Grades
            .Where(g => g.LecturerId == lecturerId)
            .Select(g => g.StudentId)
            .Distinct()
            .ToList();
        return _ = context.Students
            .Where(s => studentIds.Contains(s.Id))
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

