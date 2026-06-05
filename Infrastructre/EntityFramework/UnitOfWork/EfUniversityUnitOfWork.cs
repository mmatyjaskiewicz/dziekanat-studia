using Core.Repositories;
using Core.UnitOfWork;
using Infrastructre.EntityFramework.Context;

namespace Infrastructre.EntityFramework.UnitOfWork;
public class EfUniversityUnitOfWork(IStudentRepository students, ILecturerRepository lecturers, IGradeRepository grades, IGradeChangeHistoryRepository gradeChangeHistories, ICourseRepository courses,
    IDegreeProgramRepository degreePrograms,
    IAcademicYearRepository academicYears,
    UniversityDbContext context) : IUniversityUnitOfWork
{
    public IStudentRepository Students => students;
    public ILecturerRepository Lecturers => lecturers;
    public IGradeRepository Grades => grades;
    public IGradeChangeHistoryRepository GradeChangeHistories => gradeChangeHistories;
    public ICourseRepository Courses => courses;
    public IDegreeProgramRepository DegreePrograms => degreePrograms;
    public IAcademicYearRepository AcademicYears => academicYears;
    public ValueTask DisposeAsync() => context.DisposeAsync();
    public Task<int> SaveChangesAsync() => context.SaveChangesAsync();
    public async Task BeginTransactionAsync() => await context.Database.BeginTransactionAsync();
    public async Task CommitTransactionAsync() => await context.Database.CommitTransactionAsync();
    public async Task RollbackTransactionAsync() => await context.Database.RollbackTransactionAsync();
}

