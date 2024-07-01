using DMoreno.CashFlowControl.Domain.Entities;
using System.Linq.Expressions;

namespace DMoreno.CashFlowControl.Domain.Interfaces.Repositories.Base;

public interface IBaseRepository<TEntity> where TEntity : Entity
{
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity?> UpdateAsync(TEntity updated, Guid key);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<bool> AreThereAsync(Expression<Func<TEntity, bool>> predicate);
    Task SaveChangesAsync();
}