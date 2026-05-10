using Core.Entities;
using Core.Repositories;

namespace Infrastructre.Memory;

// Implementacja repozytorium Lecturer w pamięci z przykładowymi danymi.
public class InMemoryLecturerRepository : MemoryGenericRepository<Lecturer>, ILecturerRepository
{
    public InMemoryLecturerRepository()
    {
        var kowalski = new Lecturer
        {
            FirstName = "Jan",
            LastName = "Kowalski",
            Email = "jan.kowalski@wsei.edu.pl",
            NationalId = "80010111111",
            Title = "dr",
            Faculty = "Wydział Informatyki"
        };
        var nowak = new Lecturer
        {
            FirstName = "Anna",
            LastName = "Nowak",
            Email = "anna.nowak@wsei.edu.pl",
            NationalId = "81020222222",
            Title = "dr hab.",
            Faculty = "Wydział Matematyki"
        };

        AddAsync(kowalski).Wait();
        AddAsync(nowak).Wait();
    }

    public Lecturer? FindByCourse(Guid courseId)
    {
        throw new NotImplementedException("Metoda do uzupełnienia w ramach pracy domowej.");
    }

    public IEnumerable<Lecturer> GetByTitle(string title)
    {
        throw new NotImplementedException("Metoda do uzupełnienia w ramach pracy domowej.");
    }

    public IEnumerable<Lecturer> GetByFaculty(string faculty)
    {
        throw new NotImplementedException("Metoda do uzupełnienia w ramach pracy domowej.");
    }
}
