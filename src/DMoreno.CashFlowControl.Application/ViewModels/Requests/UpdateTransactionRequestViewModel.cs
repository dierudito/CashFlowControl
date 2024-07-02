namespace DMoreno.CashFlowControl.Application.ViewModels.Requests;
public record UpdateTransactionRequestViewModel(
    decimal Amount,
    string? Description,
    Guid? CategoryId,
    Guid? AccountId);
