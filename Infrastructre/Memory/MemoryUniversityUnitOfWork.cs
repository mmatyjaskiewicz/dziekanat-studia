using Core.UnitOfWork;
using Core.Repositories;
namespace Infrastructre.Memory;
public class MemoryUniversityUnitOfWork(
    IStudentRepository students,
    ILecturerRepository lecturers,
    IGradeRepository grades,
    IGradeChangeHistoryRepository gradeChangeHistories,
    ICourseRepository courses,
    IDegreeProgramRepository degreePrograms,
    IAcademicYearRepository academicYears) : IUniversityUnitOfWork
{
    public IStudentRepository Students => students;
    public ILecturerRepository Lecturers => lecturers;
    public IGradeRepository Grades => grades;
    public IGradeChangeHistoryRepository GradeChangeHistories => gradeChangeHistories;
    public ICourseRepository Courses => courses;
    public IDegreeProgramRepository DegreePrograms => degreePrograms;
    public IAcademicYearRepository AcademicYears => academicYears;
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    public Task<int> SaveChangesAsync() => Task.FromResult(0);
    public Task BeginTransactionAsync() => Task.CompletedTask;
    public Task CommitTransactionAsync() => Task.CompletedTask;
    public Task RollbackTransactionAsync() => Task.CompletedTask;
}

