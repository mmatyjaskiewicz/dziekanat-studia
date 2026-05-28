using Core.Entities;
using Core.Repositories;
using Infrastructre.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
namespace Infrastructre.EntityFramework.Repositories;
public class EfLecturerRepository(UniversityDbContext context)
    : EfGenericRepository<Lecturer>(context.Lecturers), ILecturerRepository
{
    public Lecturer? FindByCourse(Guid courseId)
    {
        throw new NotImplementedException("Metoda do uzupełnienia w ramach pracy domowej.");
    }
    public IEnumerable<Lecturer> GetByTitle(string title)
    {
        return _ = context.Lecturers
            .Where(l => l.Title == title)
            .AsNoTracking()
            .ToList();
    }
    public IEnumerable<Lecturer> GetByFaculty(string faculty)
    {
        return _ = context.Lecturers
            .Where(l => l.Faculty == faculty)
            .AsNoTracking()
            .ToList();
    }
}

