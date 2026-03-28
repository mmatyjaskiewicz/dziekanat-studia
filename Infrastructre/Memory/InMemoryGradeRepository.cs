using Core.Entities;
using Core.Repositories;

namespace Infrastructre.Memory;

// Implementacja repozytorium Grade w pamięci.
public class InMemoryGradeRepository : MemoryGenericRepository<Grade>, IGradeRepository
{
}
