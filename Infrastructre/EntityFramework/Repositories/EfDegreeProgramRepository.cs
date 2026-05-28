using Core.Entities;
using Core.Repositories;
using Infrastructre.EntityFramework.Context;
namespace Infrastructre.EntityFramework.Repositories;
public class EfDegreeProgramRepository(UniversityDbContext context)
    : EfGenericRepository<DegreeProgram>(context.DegreePrograms), IDegreeProgramRepository
{
}

