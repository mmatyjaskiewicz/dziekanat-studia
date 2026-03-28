using Core.Entities;
using Core.Repositories;

namespace Infrastructre.Memory;

// Implementacja repozytorium Lecturer w pamięci - metody zgłaszają NotImplementedException
// zgodnie z zaleceniem - zostaną uzupełnione w ramach pracy domowej.
public class InMemoryLecturerRepository : MemoryGenericRepository<Lecturer>, ILecturerRepository
{
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
