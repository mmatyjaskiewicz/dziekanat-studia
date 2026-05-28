using Core.Entities;
using Core.Repositories;
using Infrastructre.EntityFramework.Context;
namespace Infrastructre.EntityFramework.Repositories;
public class EfGradeRepository(UniversityDbContext context)
    : EfGenericRepository<Grade>(context.Grades), IGradeRepository
{
}

