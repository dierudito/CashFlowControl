namespace DMoreno.CashFlowControl.Application.ViewModels.Requests;

public record AddTransactionRequestViewModel(
    decimal Amount,
    string? Description);