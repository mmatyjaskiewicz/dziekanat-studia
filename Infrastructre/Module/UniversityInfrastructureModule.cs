using Core.Authorization;
using Core.Repositories;
using Core.Security;
using Core.Services;
using Core.UnitOfWork;
using Infrastructre.EntityFramework.Context;
using Infrastructre.EntityFramework.Entities;
using Infrastructre.EntityFramework.Repositories;
using Infrastructre.EntityFramework.UnitOfWork;
using Infrastructre.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
namespace Infrastructre.Module;
public static class UniversityInfrastructureModule
{
    public static IServiceCollection AddUniversityEfModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IStudentRepository, EfStudentRepository>();
        services.AddScoped<ILecturerRepository, EfLecturerRepository>();
        services.AddScoped<IGradeRepository, EfGradeRepository>();
        services.AddScoped<IGradeChangeHistoryRepository, EfGradeChangeHistoryRepository>();
        services.AddScoped<ICourseRepository, EfCourseRepository>();
        services.AddScoped<IDegreeProgramRepository, EfDegreeProgramRepository>();
        services.AddScoped<IAcademicYearRepository, EfAcademicYearRepository>();
        services.AddScoped<IUniversityUnitOfWork, EfUniversityUnitOfWork>();
        services.AddDbContext<UniversityDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DziekanatDb")
                ?? "Data Source=dziekanat.db")
                .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));
        services.AddDbContext<UniversityIdentityDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DziekanatDb")
                ?? "Data Source=dziekanat.db")
                .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));
        services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<UniversityIdentityDbContext>()
            .AddDefaultTokenProviders();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ILecturerService, LecturerService>();
        services.AddScoped<IStudentImportService, StudentImportService>();
        services.AddScoped<IDataSeeder, Infrastructre.Services.IdentityDbSeeder>();
        return services;
    }
    public static IServiceCollection AddJwt(this IServiceCollection services, JwtSettings jwtOptions)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = jwtOptions.GetSymmetricKey(),
                    ClockSkew = TimeSpan.Zero
                };
            });
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AppPolicies.AdminOnly.ToString(), policy =>
                policy.RequireRole(UserRole.Administrator.ToString()));
            options.AddPolicy(AppPolicies.DeanOffice.ToString(), policy =>
                policy.RequireRole(UserRole.DeanOfficeWorker.ToString(), UserRole.Administrator.ToString()));
            options.AddPolicy(AppPolicies.LecturerOnly.ToString(), policy =>
                policy.RequireRole(UserRole.Lecturer.ToString(), UserRole.Administrator.ToString()));
            options.AddPolicy(AppPolicies.StudentOnly.ToString(), policy =>
                policy.RequireRole(UserRole.Student.ToString(), UserRole.Administrator.ToString()));
            options.AddPolicy(AppPolicies.ActiveUser.ToString(), policy =>
                policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("status", SystemUserStatus.Active.ToString()));
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
        return services;
    }
}

