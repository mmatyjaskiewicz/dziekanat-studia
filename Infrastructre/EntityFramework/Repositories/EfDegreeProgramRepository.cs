using Core.Entities;
using Core.Repositories;
using Infrastructre.EntityFramework.Context;

namespace Infrastructre.EntityFramework.Repositories;

// Implementacja repozytorium DegreeProgram z użyciem Entity Framework Core.
public class EfDegreeProgramRepository(UniversityDbContext context)
    : EfGenericRepository<DegreeProgram>(context.DegreePrograms), IDegreeProgramRepository
{
}
