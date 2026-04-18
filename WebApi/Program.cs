using Core.Module;
using Core.Repositories;
using Core.Services;
using Core.UnitOfWork;
using FluentValidation.AspNetCore;
using Infrastructre.Memory;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        // Rejestracja automatycznej walidacji (ASP.NET Core filter).
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddOpenApi();

        // Moduł Students - rejestracja walidatorów.
        builder.Services.AddStudentsModule(builder.Configuration);

        // Rejestracja repozytoriów w pamięci (singleton - dane przechowywane są w pamięci).
        builder.Services.AddSingleton<IStudentRepository, InMemoryStudentRepository>();
        builder.Services.AddSingleton<ILecturerRepository, InMemoryLecturerRepository>();
        builder.Services.AddSingleton<IGradeRepository, InMemoryGradeRepository>();
        builder.Services.AddSingleton<ICourseRepository, InMemoryCourseRepository>();
        builder.Services.AddSingleton<IDegreeProgramRepository, InMemoryDegreeProgramRepository>();

        // Rejestracja jednostki pracy.
        builder.Services.AddSingleton<IUniversityUnitOfWork, MemoryUniversityUnitOfWork>();

        // Rejestracja serwisu studentów.
        builder.Services.AddSingleton<IStudentService, MemoryStudentService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
