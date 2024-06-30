using DMoreno.CashFlowControl.Application.ViewModels.Enums;

namespace DMoreno.CashFlowControl.Application.ViewModels.Requests;

public record AddTransactionRequestViewModel(
    DateTime Date,
    ETransactionTypeViewModel Type,
    decimal Amount,
    string? Description);