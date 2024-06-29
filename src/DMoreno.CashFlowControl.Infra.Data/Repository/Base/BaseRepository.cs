using DMoreno.CashFlowControl.Domain.Dtos;
using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories.Base;
using DMoreno.CashFlowControl.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DMoreno.CashFlowControl.Infra.Data.Repository.Base;
public class BaseRepository<TEntity> : IDisposable, IBaseRepository<TEntity> where TEntity : Entity, new()
{
    protected CashFlowControlDbContext _db;
    protected DbSet<TEntity> _dbSet;

    public BaseRepository(CashFlowControlDbContext db)
    {
        _db = db;
        _dbSet = _db.Set<TEntity>();
    }

    public void DetachLocal(Func<TEntity, bool> predicate)
    {
        var local = _db.Set<TEntity>().Local.FirstOrDefault(predicate);
        if (local != null)
        {
            _db.Entry(local).State = EntityState.Detached;
        }
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        await Task.Yield();
        _dbSet.Update(entity);
        return entity;
    }

    public async Task<TEntity?> UpdateAsync(TEntity updated, int key)
    {
        if (updated == null)
            return null;

        TEntity existing = await _dbSet.FindAsync(key);
        if (existing != null)
            _db.Entry(existing).CurrentValues.SetValues(updated);

        return existing;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id).ConfigureAwait(false);

        if (entity != null)
            _dbSet.Remove(entity);
    }

    public async Task DeleteRangeAsync(Expression<Func<TEntity, bool>>? filter = null)
    {
        var (items, _) = await GetAsync(filter).ConfigureAwait(false);
        _dbSet.RemoveRange(items);
    }

    public async Task DeleteAsync(TEntity entity)
    {
        await Task.Yield();
        _dbSet.Remove(entity);
    }

    public async Task TruncateAsync()
    {
        var name = _db.Model.FindEntityType(typeof(TEntity));
        await _db.Database.ExecuteSqlRawAsync($"truncate table {name.Name}").ConfigureAwait(false);
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id) =>
        await _dbSet.FindAsync(id);

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync() =>
        await _dbSet.ToListAsync().ConfigureAwait(false);

    public virtual async Task<(IEnumerable<TEntity> items, int count)> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        PaginationInputDto? pagination = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        await Task.Yield();
        var query = _dbSet.AsQueryable();
        int count = 0;

        if (filter != null)
            query = query.Where(filter);

        if (includes != null)
            query = includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        count = query.AsNoTracking().Count();

        if (pagination != null)
            query = query.Skip(pagination.Skip()).Take(pagination.Take());

        return (query.AsNoTracking().AsEnumerable(), count);
    }

    public virtual async Task<IEnumerable<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var result = _dbSet.AsNoTracking().Where(predicate);
        return await result.ToListAsync().ConfigureAwait(false);
    }

    public virtual async Task<IEnumerable<TEntity>> GetForUpdateAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var result = _dbSet.Where(predicate);
        return await result.ToListAsync().ConfigureAwait(false);
    }

    public void Dispose()
    {
        _db?.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> expression) => 
        await _dbSet.SingleOrDefaultAsync(expression).ConfigureAwait(false);

    public async Task<int> SaveChangesAsync() =>
        await _db.SaveChangesAsync().ConfigureAwait(false);
}
