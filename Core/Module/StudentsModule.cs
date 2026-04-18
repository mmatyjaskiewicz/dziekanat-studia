using Core.Services;
using Core.UnitOfWork;
using Core.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Core.Module;

// Klasa rozszerzająca kontroler o komponenty modułu Students.
// Walidatory i automatyczna walidacja są rejestrowane w module WebApi.
public static class StudentsModule
{
    public static IServiceCollection AddStudentsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Rejestracja walidatorów z assembly zawierającego walidator StudentCreateDtoValidator.
        services.AddValidatorsFromAssemblyContaining<StudentCreateDtoValidator>();
        return services;
    }
}
