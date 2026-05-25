using Core.Entities;
using Core.Repositories;
using Infrastructre.EntityFramework.Context;

namespace Infrastructre.EntityFramework.Repositories;

// Implementacja repozytorium AcademicYear z użyciem Entity Framework Core.
public class EfAcademicYearRepository(UniversityDbContext context)
    : EfGenericRepository<AcademicYear>(context.AcademicYears), IAcademicYearRepository
{
}
