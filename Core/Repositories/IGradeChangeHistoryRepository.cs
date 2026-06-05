using Core.Entities;
namespace Core.Repositories;
public interface IGradeChangeHistoryRepository : IGenericRepositoryAsync<GradeChangeHistory>
{
    Task<IEnumerable<GradeChangeHistory>> GetByGradeIdAsync(Guid gradeId);
}
