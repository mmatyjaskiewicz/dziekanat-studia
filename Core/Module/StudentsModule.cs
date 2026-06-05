using Core.Services;
using Core.UnitOfWork;
using Core.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
namespace Core.Module;
public static class StudentsModule
{
    public static IServiceCollection AddStudentsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<StudentCreateDtoValidator>();
        return services;
    }
}

