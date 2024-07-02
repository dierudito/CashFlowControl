using DMoreno.CashFlowControl.Domain.Entities;

namespace DMoreno.CashFlowControl.Domain.Interfaces.Services;

public interface ICategoryService
{
    Task<Category> AddAsync(Category category);

    Task<Category?> UpdateAsync(Category category, Guid idCategory);

    Task DeleteAsync(Guid id);
}
