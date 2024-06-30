using Bogus;
using DMoreno.CashFlowControl.Application.ViewModels.Enums;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;

namespace DMoreno.CashFlowControl.UnityTests.Shared.Builders;

public class AddTransactionResponseViewModelBuilder
{
    public Guid Id { get; private set; }
    public string Date { get; private set; } = null!;
    public ETransactionTypeViewModel Type { get; private set; }
    public string Amount { get; private set; } = null!;
    public string? Description { get; private set; }

    public AddTransactionResponseViewModelBuilder()
    {
        var faker = new Faker();

        WithId(Guid.NewGuid());
        WithDate(faker.Date.Recent());
        WithType(faker.PickRandom<ETransactionTypeViewModel>());
        WithAmount(faker.Finance.Amount());
        WithDescription(faker.Lorem.Paragraph());
    }

    public AddTransactionResponseViewModelBuilder WithId(Guid id)
    {
        Id = id;
        return this;
    }

    public AddTransactionResponseViewModelBuilder WithDate(DateTime date)
    {
        Date = date.ToString("dd/MM/yyyy HH:mm");
        return this;
    }

    public AddTransactionResponseViewModelBuilder WithType(ETransactionTypeViewModel type)
    {
        Type = type;
        return this;
    }

    public AddTransactionResponseViewModelBuilder WithAmount(decimal amount)
    {
        Amount = amount.ToString("C2");
        return this;
    }

    public AddTransactionResponseViewModelBuilder WithDescription(string? description)
    {
        Description = description;
        return this;
    }

    public static AddTransactionResponseViewModelBuilder New() => new();

    public AddTransactionResponseViewModel Build() => new(
        Id,
        Date,
        Type,
        Amount,
        Description
    );
}
