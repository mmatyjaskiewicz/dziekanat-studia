using Core.Entities;
using Core.Repositories;

namespace Infrastructre.Memory;

// Implementacja repozytorium Course w pamięci.
public class InMemoryCourseRepository : MemoryGenericRepository<Course>, ICourseRepository
{
}
