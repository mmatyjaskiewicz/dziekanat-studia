using Core.UnitOfWork;
using Core.Repositories;

namespace Infrastructre.Memory;

// Implementacja jednostki pracy w pamięci. Metody specyficzne dla EF
// (transakcje, save changes) są no-op, ponieważ dane przechowywane są w słowniku.
public class MemoryUniversityUnitOfWork(
    IStudentRepository students,
    ILecturerRepository lecturers,
    IGradeRepository grades,
    ICourseRepository courses,
    IDegreeProgramRepository degreePrograms,
    IAcademicYearRepository academicYears) : IUniversityUnitOfWork
{
    public IStudentRepository Students => students;
    public ILecturerRepository Lecturers => lecturers;
    public IGradeRepository Grades => grades;
    public ICourseRepository Courses => courses;
    public IDegreeProgramRepository DegreePrograms => degreePrograms;
    public IAcademicYearRepository AcademicYears => academicYears;

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    public Task<int> SaveChangesAsync() => Task.FromResult(0);
    public Task BeginTransactionAsync() => Task.CompletedTask;
    public Task CommitTransactionAsync() => Task.CompletedTask;
    public Task RollbackTransactionAsync() => Task.CompletedTask;
}
