using DMoreno.CashFlowControl.Application.ViewModels.Enums;

namespace DMoreno.CashFlowControl.Application.ViewModels.Requests;
public class UpdateTransactionRequestViewModel
{
    public DateTime Date { get; set; }
    public ETransactionTypeViewModel Type { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? AccountId { get; set; }
}
