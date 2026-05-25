using Core.Entities;
using Core.Repositories;
using Infrastructre.EntityFramework.Context;

namespace Infrastructre.EntityFramework.Repositories;

// Implementacja repozytorium Grade z użyciem Entity Framework Core.
public class EfGradeRepository(UniversityDbContext context)
    : EfGenericRepository<Grade>(context.Grades), IGradeRepository
{
}
