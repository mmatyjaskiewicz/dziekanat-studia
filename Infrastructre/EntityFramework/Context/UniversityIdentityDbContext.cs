using Core.Security;
using Infrastructre.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructre.EntityFramework.Context;

// Kontekst bazy danych dla tożsamości użytkowników (Identity) oraz encji domenowych.
public class UniversityIdentityDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public UniversityIdentityDbContext() { }
    public UniversityIdentityDbContext(DbContextOptions<UniversityIdentityDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=university.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Konfiguracja właściwości użytkownika.
        builder.Entity<AppUser>(entity =>
        {
            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.LastName).HasMaxLength(100);
            entity.Property(u => u.Department).HasMaxLength(100);
            entity.HasIndex(u => u.Email).IsUnique();
        });

        builder.Entity<AppRole>(entity =>
        {
            entity.Property(r => r.Name).HasMaxLength(50);
            entity.Property(r => r.Description).HasMaxLength(200);
        });

        // Wartości początkowe ról.
        builder.Entity<AppRole>().HasData(
            new AppRole(UserRole.Administrator.ToString(), "Pełny dostęp do systemu."),
            new AppRole(UserRole.DeanOfficeWorker.ToString(), "Pracownik dziekanatu."),
            new AppRole(UserRole.Lecturer.ToString(), "Wykładowca."),
            new AppRole(UserRole.Student.ToString(), "Student.")
        );
    }
}
