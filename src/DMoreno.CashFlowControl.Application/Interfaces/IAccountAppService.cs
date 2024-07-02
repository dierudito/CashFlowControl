using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;

namespace DMoreno.CashFlowControl.Application.Interfaces;

public interface IAccountAppService
{
    Task<Response<AccountResponseViewModel>>
        AddAsync(AccountRequestViewModel accountRequestViewModel);

    Task<Response<bool>>
        UpdateAsync(AccountRequestViewModel accountRequestViewModel, Guid idAccount);

    Task<Response<bool>>
        DeleteAsync(Guid idAccount);

    Task<Response<AccountResponseViewModel>>
        GetByIdAsync(Guid id);

    Task<Response<List<AccountResponseViewModel>>> GetAllAsync();
}