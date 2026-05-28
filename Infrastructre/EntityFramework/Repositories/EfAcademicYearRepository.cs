using Core.Entities;
using Core.Repositories;
using Infrastructre.EntityFramework.Context;
namespace Infrastructre.EntityFramework.Repositories;
public class EfAcademicYearRepository(UniversityDbContext context)
    : EfGenericRepository<AcademicYear>(context.AcademicYears), IAcademicYearRepository
{
}

