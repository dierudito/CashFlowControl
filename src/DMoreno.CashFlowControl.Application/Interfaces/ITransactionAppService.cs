using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;

namespace DMoreno.CashFlowControl.Application.Interfaces;
public interface ITransactionAppService
{
    Task<Response<AddTransactionResponseViewModel>> 
        AddAsync(AddTransactionRequestViewModel addTransactionRequestViewModel);

    Task<Response<bool>> 
        UpdateAsync(UpdateTransactionRequestViewModel updateTransactionRequestViewModel, Guid idTransaction);

    Task<Response<bool>>
        DeleteAsync(Guid idTransaction);

    Task<Response<GetTransactionByIdResponseViewModel>>
        GetByIdAsync(Guid id);
}
