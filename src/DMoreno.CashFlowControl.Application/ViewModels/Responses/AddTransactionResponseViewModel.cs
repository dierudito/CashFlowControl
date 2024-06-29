using DMoreno.CashFlowControl.Application.ViewModels.Enums;

namespace DMoreno.CashFlowControl.Application.ViewModels.Responses;

public record AddTransactionResponseViewModel(
    Guid Id,
    string Date,
    ETransactionTypeViewModel Type,
    string Amount,
    string? Description);
