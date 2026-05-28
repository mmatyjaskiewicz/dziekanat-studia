using Core.Authorization;
using Core.Module;
using Core.Services;
using FluentValidation.AspNetCore;
using Infrastructre.Module;
using Infrastructre.Security;
using Microsoft.AspNetCore.Authorization;
using WebApi.Middleware;
namespace WebApi;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddOpenApi();
        builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();
        builder.Services.AddProblemDetails();
        var jwtSettings = new JwtSettings(builder.Configuration);
        builder.Services.AddSingleton(jwtSettings);
        builder.Services.AddScoped<IAuthService, Infrastructre.Services.AuthService>();
        builder.Services.AddStudentsModule(builder.Configuration);
        builder.Services.AddUniversityEfModule(builder.Configuration);
        builder.Services.AddJwt(jwtSettings);
        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            using var scope = app.Services.CreateScope();
            var seeders = scope.ServiceProvider.GetServices<IDataSeeder>().OrderBy(s => s.Order);
            foreach (var seeder in seeders)
                await seeder.SeedAsync();
        }
        app.UseExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpsRedirection();
        app.MapControllers();
        app.Run();
    }
}
