using Core.Security;
using Core.Services;
using Infrastructre.EntityFramework.Context;
using Infrastructre.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructre.Services;

public class IdentityDbSeeder : IDataSeeder
{
    public int Order => 1;

    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly ILogger<IdentityDbSeeder> _logger;

    public IdentityDbSeeder(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ILogger<IdentityDbSeeder> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (!_userManager.Users.Any())
            await SeedRolesAsync();
        await SeedUsersAsync();
    }

    private async Task SeedRolesAsync()
    {
        var roles = new[]
        {
            new AppRole(UserRole.Administrator.ToString(), "Pełny dostęp do systemu."),
            new AppRole(UserRole.DeanOfficeWorker.ToString(), "Pracownik dziekanatu."),
            new AppRole(UserRole.Lecturer.ToString(), "Wykładowca."),
            new AppRole(UserRole.Student.ToString(), "Student.")
        };

        foreach (var role in roles)
        {
            if (await _roleManager.RoleExistsAsync(role.Name!))
                continue;

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                _logger.LogError("Błąd tworzenia roli {Role}: {Errors}", role.Name, FormatErrors(result));
        }
    }

    private async Task SeedUsersAsync()
    {
        var users = new[]
        {
            new SeedUser(
                Id: "F5BADE14-6CC8-42A2-9A44-9842DA2D9280",
                Email: "admin@app.pl",
                FirstName: "Adam",
                LastName: "Administrator",
                Department: "IT",
                Password: "Admin@123!",
                Role: UserRole.Administrator
            ),
            new SeedUser(
                Id: "93A7FFDD-057F-4021-9C68-FE06951FFA65",
                Email: "jan.kowalski@app.pl",
                FirstName: "Jan",
                LastName: "Kowalski",
                Department: "DeanOffice",
                Password: "Dean@123!",
                Role: UserRole.DeanOfficeWorker
            ),
            new SeedUser(
                Id: "3D4769E2-1C75-43E1-A5BB-1F71C68E9F57",
                Email: "anna.nowak@app.pl",
                FirstName: "Anna",
                LastName: "Nowak",
                Department: "Math",
                Password: "Lect@123!",
                Role: UserRole.Lecturer
            )
        };

        foreach (var seedUser in users)
        {
            var existing = await _userManager.FindByEmailAsync(seedUser.Email);
            if (existing is null)
            {
                var byName = await _userManager.FindByNameAsync(seedUser.Email);
                if (byName is not null)
                {
                    byName.Email = seedUser.Email;
                    byName.NormalizedEmail = seedUser.Email.ToUpperInvariant();
                    existing = byName;
                }
            }
            if (existing is not null && string.IsNullOrEmpty(existing.NormalizedEmail))
            {
                existing.NormalizedEmail = seedUser.Email.ToUpperInvariant();
            }
            if (existing is null)
            {
                await CreateUserAsync(seedUser);
            }
            else
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existing);
                var resetResult = await _userManager.ResetPasswordAsync(existing, token, seedUser.Password);
                if (!resetResult.Succeeded)
                {
                    _logger.LogError("Błąd resetu hasła {Email}: {Errors}",
                        seedUser.Email, FormatErrors(resetResult));
                    continue;
                }
                existing.NormalizedEmail = seedUser.Email.ToUpperInvariant();
                await _userManager.UpdateAsync(existing);
                if (!await _userManager.IsInRoleAsync(existing, seedUser.Role.ToString()))
                {
                    await _userManager.AddToRoleAsync(existing, seedUser.Role.ToString());
                }
                _logger.LogInformation("Zresetowano hasło i potwierdzono rolę dla {Email}.", seedUser.Email);
            }
        }
    }

    private async Task CreateUserAsync(SeedUser seedUser)
    {
        if (await _userManager.FindByEmailAsync(seedUser.Email) is not null)
        {
            _logger.LogInformation("Użytkownik {Email} już istnieje.", seedUser.Email);
            return;
        }

        var user = new AppUser
        {
            Id = seedUser.Id,
            Email = seedUser.Email,
            FullName = $"{seedUser.FirstName} {seedUser.LastName}",
            EmailConfirmed = true,
            LockoutEnabled = true,
            LockoutEnd = null,
            AccessFailedCount = 0,
            TwoFactorEnabled = false,
            PhoneNumberConfirmed = false,
            FirstName = seedUser.FirstName,
            LastName = seedUser.LastName,
            Department = seedUser.Department,
            Status = SystemUserStatus.Active,
        };
        user.Activate();

        await _userManager.SetUserNameAsync(user, seedUser.Email);
        await _userManager.SetEmailAsync(user, seedUser.Email);
        await _userManager.UpdateNormalizedEmailAsync(user);

        var createResult = await _userManager.CreateAsync(user, seedUser.Password);
        if (!createResult.Succeeded)
        {
            _logger.LogError("Błąd tworzenia użytkownika {Email}: {Errors}", seedUser.Email, FormatErrors(createResult));
            return;
        }

        var roleResult = await _userManager.AddToRoleAsync(user, seedUser.Role.ToString());
        if (!roleResult.Succeeded)
        {
            _logger.LogError("Błąd przypisania roli {Role} dla {Email}: {Errors}", seedUser.Role, seedUser.Email, FormatErrors(roleResult));
            return;
        }

        _logger.LogInformation("Utworzono użytkownika {Email} z rolą {Role}.", seedUser.Email, seedUser.Role);
    }

    private static string FormatErrors(IdentityResult result) =>
        string.Join("; ", result.Errors.Select(e => e.Description));
}

internal record SeedUser(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    string Department,
    string Password,
    UserRole Role
);
