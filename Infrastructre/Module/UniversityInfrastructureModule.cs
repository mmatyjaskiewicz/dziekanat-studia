using Core.Repositories;
using Core.Services;
using Core.UnitOfWork;
using Infrastructre.EntityFramework.Context;
using Infrastructre.EntityFramework.Repositories;
using Infrastructre.EntityFramework.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructre.Module;

// Klasa rozszerzająca IoC o komponenty modułu Infrastructure.
// W zadaniu 5 docelową implementacją jest Entity Framework (Sqlite).
public static class UniversityInfrastructureModule
{
    public static IServiceCollection AddUniversityEfModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IStudentRepository, EfStudentRepository>();
        services.AddScoped<ILecturerRepository, EfLecturerRepository>();
        services.AddScoped<IGradeRepository, EfGradeRepository>();
        services.AddScoped<ICourseRepository, EfCourseRepository>();
        services.AddScoped<IDegreeProgramRepository, EfDegreeProgramRepository>();
        services.AddScoped<IAcademicYearRepository, EfAcademicYearRepository>();

        services.AddScoped<IUniversityUnitOfWork, EfUniversityUnitOfWork>();
        services.AddDbContext<UniversityDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("UniversityDb")
                ?? "Data Source=university.db"));

        services.AddScoped<IStudentService, StudentService>();
        return services;
    }
}
