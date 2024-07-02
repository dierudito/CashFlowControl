using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Domain.Interfaces.Services;

namespace DMoreno.CashFlowControl.Domain.Services;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task<Category> AddAsync(Category category) =>
        await categoryRepository.AddAsync(category);

    public async Task<Category?> UpdateAsync(Category category, Guid idCategory) =>
        await categoryRepository.UpdateAsync(category, idCategory);

    public async Task DeleteAsync(Guid id) =>
        await categoryRepository.DeleteAsync(id);
}
