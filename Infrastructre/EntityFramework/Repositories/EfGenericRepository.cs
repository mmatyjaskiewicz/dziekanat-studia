using Core.Entities;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Infrastructre.EntityFramework.Repositories;
public class EfGenericRepository<T>(DbSet<T> set) : IGenericRepositoryAsync<T> where T : EntityBase
{
    public virtual async Task<T?> FindByIdAsync(Guid id)
    {
        return await set.FindAsync(id);
    }
    public async Task<IEnumerable<T>> FindAllAsync()
    {
        return await set.ToListAsync();
    }
    public async Task<Core.Common.PagedResult<T>> FindPagedAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        var total = await set.CountAsync();
        var items = await set
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return new Core.Common.PagedResult<T>(items, total, page, pageSize);
    }
    public async Task<T> AddAsync(T entity)
    {
        var entry = await set.AddAsync(entity);
        return entry.Entity;
    }
    public Task<T> UpdateAsync(T entity)
    {
        var entry = set.Update(entity);
        return Task.FromResult(entry.Entity);
    }
    public async Task RemoveByIdAsync(Guid id)
    {
        var entity = await set.FindAsync(id);
        if (entity is not null) set.Remove(entity);
    }
}

