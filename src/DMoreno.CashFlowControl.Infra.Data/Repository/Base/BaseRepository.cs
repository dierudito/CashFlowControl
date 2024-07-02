using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories.Base;
using DMoreno.CashFlowControl.Infra.Data.Context.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DMoreno.CashFlowControl.Infra.Data.Repository.Base;
public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Entity, new()
{
    protected CashFlowControlDbContext _db;
    protected DbSet<TEntity> _dbSet;

    public BaseRepository(CashFlowControlDbContext db)
    {
        _db = db;
        _dbSet = _db.Set<TEntity>();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public virtual async Task<TEntity?> UpdateAsync(TEntity updated, Guid key)
    {
        if (updated == null)
            return null;

        var existing = await _dbSet.FindAsync(key);
        if (existing != null)
        {
            updated.Id = key;
            _db.Entry(existing).CurrentValues.SetValues(updated);
        }

        return existing;
    }

    public async Task DeleteAsync(Guid id)
    {
        await Task.Yield();
        var entity = new TEntity { Id = id };
        _dbSet.Remove(entity);
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id) =>
        await _dbSet.FirstOrDefaultAsync(entity => entity.Id == id);

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public async Task<bool> AreThereAsync(Expression<Func<TEntity, bool>> predicate) =>
        await _dbSet.AsNoTracking().AnyAsync(predicate);

    public async Task SaveChangesAsync() =>
        await _db.SaveChangesAsync();
}
