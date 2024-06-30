using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;

namespace DMoreno.CashFlowControl.Application.Interfaces;
public interface ITransactionAppService
{
    Task<Response<AddTransactionResponseViewModel>> 
        AddAsync(AddTransactionRequestViewModel addTransactionRequestViewModel);

    Task<Response<GetTransactionByIdResponseViewModel>>
        GetByIdAsync(Guid id);
}
