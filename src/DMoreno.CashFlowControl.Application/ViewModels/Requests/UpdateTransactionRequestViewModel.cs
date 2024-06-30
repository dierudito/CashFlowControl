using DMoreno.CashFlowControl.Application.ViewModels.Enums;

namespace DMoreno.CashFlowControl.Application.ViewModels.Requests;
public record UpdateTransactionRequestViewModel(
    DateTime Date, 
    ETransactionTypeViewModel Type, 
    decimal Amount, 
    string? Description, 
    Guid? CategoryId, 
    Guid? AccountId);
