using Core.Entities;
using Core.Repositories;
using Infrastructre.EntityFramework.Context;

namespace Infrastructre.EntityFramework.Repositories;

// Implementacja repozytorium Course z użyciem Entity Framework Core.
public class EfCourseRepository(UniversityDbContext context)
    : EfGenericRepository<Course>(context.Courses), ICourseRepository
{
}
