using Core.Repositories;
using Core.Services;
using Core.UnitOfWork;
using Infrastructre.Memory;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

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
