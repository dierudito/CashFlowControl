using Bogus;
using DMoreno.CashFlowControl.Application.ViewModels.Enums;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;

namespace DMoreno.CashFlowControl.UnityTests.Shared.Builders;

public class GetTransactionByIdResponseViewModelBuilder
{
    public Guid Id { get; private set; }
    public string Date { get; private set; } = null!;
    public ETransactionTypeViewModel Type { get; private set; }
    public string Amount { get; private set; } = null!;
    public string? Description { get; private set; }
    public Guid? CategoryId { get; private set; }
    public Guid? AccountId { get; private set; }

    public GetTransactionByIdResponseViewModelBuilder()
    {
        var faker = new Faker();

        WithId(Guid.NewGuid());
        WithDate(faker.Date.Recent());
        WithType(faker.PickRandom<ETransactionTypeViewModel>());
        WithAmount(faker.Finance.Amount());
        WithDescription(faker.Lorem.Paragraph());
        WithCategoryId(Guid.NewGuid());
        WithAccountId(Guid.NewGuid());
    }

    public GetTransactionByIdResponseViewModelBuilder WithId(Guid id)
    {
        Id = id;
        return this;
    }

    public GetTransactionByIdResponseViewModelBuilder WithDate(DateTime date)
    {
        Date = date.ToString("dd/MM/yyyy HH:mm");
        return this;
    }

    public GetTransactionByIdResponseViewModelBuilder WithType(ETransactionTypeViewModel type)
    {
        Type = type;
        return this;
    }

    public GetTransactionByIdResponseViewModelBuilder WithAmount(decimal amount)
    {
        Amount = amount.ToString("C2");
        return this;
    }

    public GetTransactionByIdResponseViewModelBuilder WithDescription(string? description)
    {
        Description = description;
        return this;
    }

    public GetTransactionByIdResponseViewModelBuilder WithCategoryId(Guid? categoryId)
    {
        CategoryId = categoryId;
        return this;
    }

    public GetTransactionByIdResponseViewModelBuilder WithAccountId(Guid? accountId)
    {
        AccountId = accountId;
        return this;
    }


    public static GetTransactionByIdResponseViewModelBuilder New() => new();

    public GetTransactionByIdResponseViewModel Build() => new(
        Id,
        Date,
        Type,
        Amount,
        Description,
        CategoryId,
        AccountId
    );
}
