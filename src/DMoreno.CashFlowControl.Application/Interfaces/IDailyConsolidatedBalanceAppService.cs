using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;

namespace DMoreno.CashFlowControl.Application.Interfaces;
public interface IDailyConsolidatedBalanceAppService
{
    Task<Response<List<DailyConsolidatedBalanceResponseViewModel>>>
        GetByPeriodAsync(DailyConsolidatedBalanceRequestViewModel request);
}
