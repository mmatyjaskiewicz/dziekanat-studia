using Core.Entities;
using Core.Repositories;
using Infrastructre.EntityFramework.Context;
namespace Infrastructre.EntityFramework.Repositories;
public class EfCourseRepository(UniversityDbContext context)
    : EfGenericRepository<Course>(context.Courses), ICourseRepository
{
}

