using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructre.EntityFramework.Context;

// Kontekst bazy danych dla aplikacji dziekanatu.
// Konfiguruje związki między encjami oraz dostarcza wartości początkowe.
public class UniversityDbContext : DbContext
{
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Lecturer> Lecturers => Set<Lecturer>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<DegreeProgram> DegreePrograms => Set<DegreeProgram>();
    public DbSet<AcademicYear> AcademicYears => Set<AcademicYear>();

    public UniversityDbContext() { }

    public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=university.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfiguracja mapowania TPH (Table-Per-Hierarchy) dla dziedziczenia Person.
        modelBuilder.Entity<Person>()
            .HasDiscriminator<string>("PersonType")
            .HasValue<Student>("Student")
            .HasValue<Lecturer>("Lecturer");

        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(s => s.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(s => s.LastName).HasMaxLength(200).IsRequired();
            entity.Property(s => s.Email).HasMaxLength(200).IsRequired();
            entity.Property(s => s.NationalId).HasMaxLength(11).IsRequired();
            entity.Property(s => s.StudentNumber).HasMaxLength(20).IsRequired();
            entity.Property(s => s.Status).HasConversion<string>();
        });

        modelBuilder.Entity<Lecturer>(entity =>
        {
            entity.Property(l => l.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(l => l.LastName).HasMaxLength(200).IsRequired();
            entity.Property(l => l.Email).HasMaxLength(200).IsRequired();
            entity.Property(l => l.NationalId).HasMaxLength(11).IsRequired();
            entity.Property(l => l.Title).HasMaxLength(20);
            entity.Property(l => l.Faculty).HasMaxLength(100);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(c => c.Code).HasMaxLength(20).IsRequired();
            entity.Property(c => c.Name).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<DegreeProgram>(entity =>
        {
            entity.Property(d => d.Code).HasMaxLength(20).IsRequired();
            entity.Property(d => d.Name).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<AcademicYear>(entity =>
        {
            entity.Property(a => a.Name).HasMaxLength(20).IsRequired();
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.Property(g => g.Value).HasConversion<string>();
            entity.HasOne<Student>().WithMany(s => s.Grades).HasForeignKey(g => g.StudentId);
        });
    }
}
