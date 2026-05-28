using Core.Common;
using Core.Entities;
using Core.Repositories;
namespace Infrastructre.Memory;
public class MemoryGenericRepository<T> : IGenericRepositoryAsync<T>
    where T : EntityBase
{
    protected readonly Dictionary<Guid, T> _data = new();
    public virtual Task<T?> FindByIdAsync(Guid id)
    {
        var result = _data.TryGetValue(id, out var value) ? value : null;
        return Task.FromResult(result);
    }
    public virtual Task<IEnumerable<T>> FindAllAsync()
    {
        IReadOnlyCollection<T> snapshot = _data.Values.ToList();
        return Task.FromResult<IEnumerable<T>>(snapshot);
    }
    public virtual Task<PagedResult<T>> FindPagedAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        var total = _data.Count;
        var items = _data.Values
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        return Task.FromResult(new PagedResult<T>(items, total, page, pageSize));
    }
    public virtual Task<T> AddAsync(T entity)
    {
        if (entity.Id == Guid.Empty)
            entity.Id = Guid.NewGuid();
        _data[entity.Id] = entity;
        return Task.FromResult(entity);
    }
    public virtual Task<T> UpdateAsync(T entity)
    {
        if (!_data.ContainsKey(entity.Id))
            throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with id={entity.Id} not found.");
        _data[entity.Id] = entity;
        return Task.FromResult(entity);
    }
    public virtual Task RemoveByIdAsync(Guid id)
    {
        _data.Remove(id);
        return Task.CompletedTask;
    }
}

