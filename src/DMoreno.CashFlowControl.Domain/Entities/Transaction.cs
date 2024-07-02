using DMoreno.CashFlowControl.Domain.Enums;

namespace DMoreno.CashFlowControl.Domain.Entities;
public class Transaction : Entity
{
    public ETransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? AccountId { get; set; }
    public Guid CashFlowId { get; set; }

    public virtual Category Category { get; set; }
    public virtual Account Account { get; set; }
    public virtual CashFlow CashFlow { get; set; }
}
