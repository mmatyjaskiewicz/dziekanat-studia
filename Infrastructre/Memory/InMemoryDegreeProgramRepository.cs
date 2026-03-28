using Core.Entities;
using Core.Repositories;

namespace Infrastructre.Memory;

// Implementacja repozytorium DegreeProgram w pamięci.
public class InMemoryDegreeProgramRepository : MemoryGenericRepository<DegreeProgram>, IDegreeProgramRepository
{
}
