using Core.Entities;
using Core.Repositories;
namespace Infrastructre.Memory;
public class InMemoryCourseRepository : MemoryGenericRepository<Course>, ICourseRepository
{
    public InMemoryCourseRepository()
    {
        var algebra = new Course { Code = "ALG1", Name = "Algebra liniowa", Ects = 5 };
        var programowanie = new Course { Code = "PRG1", Name = "Podstawy programowania", Ects = 6 };
        var bazy = new Course { Code = "BD1", Name = "Bazy danych", Ects = 4 };
        AddAsync(algebra).Wait();
        AddAsync(programowanie).Wait();
        AddAsync(bazy).Wait();
    }
}

