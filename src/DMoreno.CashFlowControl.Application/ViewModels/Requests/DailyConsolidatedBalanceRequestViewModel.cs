using DMoreno.CashFlowControl.Domain.Extensions;

namespace DMoreno.CashFlowControl.Application.ViewModels.Requests;
public class DailyConsolidatedBalanceRequestViewModel
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public DailyConsolidatedBalanceRequestViewModel(DateTime? startDate, DateTime? endDate)
    {
        StartDate = startDate.DateOnly();
        EndDate = endDate.DateOnly();
    }
}