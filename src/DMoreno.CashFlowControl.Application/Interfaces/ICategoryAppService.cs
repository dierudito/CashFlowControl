using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;

namespace DMoreno.CashFlowControl.Application.Interfaces;

public interface ICategoryAppService
{
    Task<Response<CategoryResponseViewModel>>
        AddAsync(CategoryRequestViewModel categoryRequestViewModel);

    Task<Response<bool>>
        UpdateAsync(CategoryRequestViewModel categoryRequestViewModel, Guid idCategory);

    Task<Response<bool>>
        DeleteAsync(Guid idCategory);

    Task<Response<CategoryResponseViewModel>>
        GetByIdAsync(Guid id);

    Task<Response<List<CategoryResponseViewModel>>> GetAllAsync();
}
