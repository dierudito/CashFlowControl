using DMoreno.CashFlowControl.Application.ViewModels.Enums;

namespace DMoreno.CashFlowControl.Application.ViewModels.Responses;

public record GetTransactionByIdResponseViewModel(
    Guid Id,
    string Date,
    ETransactionTypeViewModel Type,
    string Amount,
    string? Description,
    Guid? CategoryId,
    Guid? AccountId);
