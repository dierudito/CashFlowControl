using Bogus;
using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Enums;

namespace DMoreno.CashFlowControl.UnityTests.Shared.Builders;
public class TransactionBuilder
{
    public Guid Id { get; private set; }
    public DateTime Date { get; private set; }
    public ETransactionType Type { get; private set; }
    public decimal Amount { get; private set; }
    public string? Description { get; private set; }
    public Guid? CategoryId { get; private set; }
    public Guid? AccountId { get; private set; }
    public Guid CashFlowId { get; private set; }
    public CashFlow CashFlow { get; private set; }


    public TransactionBuilder()
    {
        var faker = new Faker();

        WithId(Guid.NewGuid());
        WithDate(faker.Date.Recent());
        WithType(faker.PickRandom<ETransactionType>());
        WithAmount(faker.Finance.Amount());
        WithDescription(faker.Lorem.Paragraph());
        WithCategoryId(Guid.NewGuid());
        WithAccountId(Guid.NewGuid());
        WithCashFlowId(Guid.NewGuid());
        WithCashFlow(CashFlowBuilder.New().Build());
    }

    public TransactionBuilder WithId(Guid id)
    {
        Id = id;
        return this;
    }

    public TransactionBuilder WithDate(DateTime date)
    {
        Date = date;
        return this;
    }
    public TransactionBuilder WithType(ETransactionType type)
    {
        Type = type;
        return this;
    }

    public TransactionBuilder WithAmount(decimal amount)
    {
        Amount = amount;
        return this;
    }

    public TransactionBuilder WithDescription(string? description)
    {
        Description = description;
        return this;
    }

    public TransactionBuilder WithCategoryId(Guid? categoryId)
    {
        CategoryId = categoryId;
        return this;
    }

    public TransactionBuilder WithAccountId(Guid? accountId)
    {
        AccountId = accountId;
        return this;
    }

    public TransactionBuilder WithCashFlowId(Guid cashFlowId)
    {
        CashFlowId = cashFlowId;
        return this;
    }

    public TransactionBuilder WithCashFlow(CashFlow cashFlow)
    {
        CashFlow = cashFlow;
        return this;
    }

    public static TransactionBuilder New() => new();

    public Transaction Build() => new()
    {
        Id = Id,
        Type = Type,
        Amount = Amount,
        Description = Description,
        CategoryId = CategoryId,
        AccountId = AccountId,
        CashFlowId = CashFlowId,
        CashFlow = CashFlow
    };
}
