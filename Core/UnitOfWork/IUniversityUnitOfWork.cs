using Core.Repositories;

namespace Core.UnitOfWork;

// Interfejs jednostki pracy grupujący repozytoria pochodzące z tego samego źródła danych.
public interface IUniversityUnitOfWork : IAsyncDisposable
{
    IStudentRepository Students { get; }
    ILecturerRepository Lecturers { get; }
    IGradeRepository Grades { get; }
    ICourseRepository Courses { get; }
    IDegreeProgramRepository DegreePrograms { get; }
    IAcademicYearRepository AcademicYears { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
