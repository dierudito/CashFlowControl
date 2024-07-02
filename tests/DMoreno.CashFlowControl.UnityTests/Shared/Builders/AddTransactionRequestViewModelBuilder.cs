using Bogus;
using DMoreno.CashFlowControl.Application.ViewModels.Enums;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;

namespace DMoreno.CashFlowControl.UnityTests.Shared.Builders;
public class AddTransactionRequestViewModelBuilder
{
    public decimal Amount { get; private set; }
    public string? Description { get; private set; }

    public AddTransactionRequestViewModelBuilder()
    {
        var faker = new Faker();

        WithAmount(faker.Finance.Amount());
        WithDescription(faker.Lorem.Paragraph());
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
        Amount,
        Description
    );
}