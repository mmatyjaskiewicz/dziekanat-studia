using Core.Entities;
using Core.Repositories;
using Infrastructre.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructre.EntityFramework.Repositories;

public class EfGradeChangeHistoryRepository(UniversityDbContext context)
    : EfGenericRepository<GradeChangeHistory>(context.GradeChangeHistories), IGradeChangeHistoryRepository
{
    public async Task<IEnumerable<GradeChangeHistory>> GetByGradeIdAsync(Guid gradeId)
    {
        return await context.GradeChangeHistories
            .Where(h => h.GradeId == gradeId)
            .AsNoTracking()
            .ToListAsync();
    }
}
