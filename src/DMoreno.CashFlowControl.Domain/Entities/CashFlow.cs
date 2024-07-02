using DMoreno.CashFlowControl.Domain.Enums;

namespace DMoreno.CashFlowControl.Domain.Entities;
public class CashFlow : Entity
{
    public DateOnly ReleaseDate { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal TotalCredits { get; set; }
    public decimal TotalDebits { get; set; }
    public decimal ClosingBalance { get; set; }

    public void SetClosingBalance() => ClosingBalance = OpeningBalance + TotalCredits - TotalDebits;

    public void IncrementTotals(ETransactionType transactionType, decimal amount)
    {
        switch (transactionType)
        {
            case ETransactionType.debit:
                TotalDebits += amount;
                break;
            case ETransactionType.credit:
                TotalCredits += amount;
                break;
        }
        SetClosingBalance();
    }

    public void UpdateTotals(ETransactionType oldTransactionType, decimal oldAmount, ETransactionType? newTransactionType = null, decimal newAmount = 0)
    {
        switch (oldTransactionType)
        {
            case ETransactionType.debit:
                TotalDebits -= oldAmount;
                break;
            case ETransactionType.credit:
                TotalCredits -= oldAmount;
                break;
        }

        if (newTransactionType is null) SetClosingBalance();
        else IncrementTotals(newTransactionType.Value, newAmount);
    }
}