using Core.Module;
using FluentValidation.AspNetCore;
using Infrastructre.Module;
using WebApi.Middleware;

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

        // Rejestracja globalnego handlera wyjątków.
        builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();
        builder.Services.AddProblemDetails();

        // Moduł Students - rejestracja walidatorów.
        builder.Services.AddStudentsModule(builder.Configuration);

        // Rejestracja warstwy infrastruktury (EF) - serwisy, repozytoria, kontekst, jednostka pracy.
        builder.Services.AddUniversityEfModule(builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        // Globalna obsługa wyjątków - musi być przed mapowaniem kontrolerów.
        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
