using DMoreno.CashFlowControl.Domain.Dtos;
using DMoreno.CashFlowControl.Domain.Entities;
using System.Linq.Expressions;

namespace DMoreno.CashFlowControl.Domain.Interfaces.Repositories.Base;

public interface IBaseRepository<TEntity> where TEntity : Entity
{
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<TEntity?> UpdateAsync(TEntity updated, int key);
    Task DeleteAsync(Guid id);
    Task DeleteAsync(TEntity entity);
    Task DeleteRangeAsync(Expression<Func<TEntity, bool>>? filter = null);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<(IEnumerable<TEntity> items, int count)> GetAsync(Expression<Func<TEntity, bool>>? filter = null,
        PaginationInputDto? pagination = null,
        params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> expression);

    Task<IEnumerable<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> predicate);

    Task<IList<TEntity>> GetForUpdateAsync(Expression<Func<TEntity, bool>> predicate);

    Task<int> SaveChangesAsync();
}