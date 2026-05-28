using Core.Entities;
using Core.Repositories;
namespace Infrastructre.Memory;
public class InMemoryAcademicYearRepository : MemoryGenericRepository<AcademicYear>, IAcademicYearRepository
{
    public InMemoryAcademicYearRepository()
    {
        var year2526 = new AcademicYear
        {
            Name = "2025/2026",
            StartDate = new DateTime(2025, 10, 1),
            EndDate = new DateTime(2026, 9, 30)
        };
        var year2425 = new AcademicYear
        {
            Name = "2024/2025",
            StartDate = new DateTime(2024, 10, 1),
            EndDate = new DateTime(2025, 9, 30)
        };
        AddAsync(year2526).Wait();
        AddAsync(year2425).Wait();
    }
}

