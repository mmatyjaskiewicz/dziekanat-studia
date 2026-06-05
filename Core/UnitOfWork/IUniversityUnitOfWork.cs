using Core.Repositories;
namespace Core.UnitOfWork;
public interface IUniversityUnitOfWork : IAsyncDisposable
{
    IStudentRepository Students { get; }
    ILecturerRepository Lecturers { get; }
    IGradeRepository Grades { get; }
    IGradeChangeHistoryRepository GradeChangeHistories { get; }
    ICourseRepository Courses { get; }
    IDegreeProgramRepository DegreePrograms { get; }
    IAcademicYearRepository AcademicYears { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}

