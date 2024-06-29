using Bogus;
using DMoreno.CashFlowControl.Application.ViewModels.Enums;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;

namespace DMoreno.CashFlowControl.UnityTests.Shared.Builders;
public class AddTransactionRequestViewModelBuilder
{
    public DateTime Date { get; private set; }
    public ETransactionTypeViewModel Type { get; private set; }
    public decimal Amount { get; private set; }
    public string? Description { get; private set; }

    public AddTransactionRequestViewModelBuilder()
    {
        var faker = new Faker();

        WithDate(faker.Date.Recent());
        WithType(faker.PickRandom<ETransactionTypeViewModel>());
        WithAmount(faker.Finance.Amount());
        WithDescription(faker.Lorem.Paragraph());
    }

    public AddTransactionRequestViewModelBuilder WithDate(DateTime date)
    {
        Date = date;
        return this;
    }

    public AddTransactionRequestViewModelBuilder WithType(ETransactionTypeViewModel type)
    {
        Type = type;
        return this;
    }

    public AddTransactionRequestViewModelBuilder WithAmount(decimal amount)
    {
        Amount = amount;
        return this;
    }

    public AddTransactionRequestViewModelBuilder WithDescription(string? description)
    {
        Description = description;
        return this;
    }

    public static AddTransactionRequestViewModelBuilder New() => new();

    public AddTransactionRequestViewModel Build() => new(
        Date,
        Type,
        Amount,
        Description
    );
}