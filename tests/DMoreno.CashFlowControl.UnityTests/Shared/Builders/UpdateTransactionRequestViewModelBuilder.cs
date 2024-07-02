using Bogus;
using DMoreno.CashFlowControl.Application.ViewModels.Enums;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;

namespace DMoreno.CashFlowControl.UnityTests.Shared.Builders;
public class UpdateTransactionRequestViewModelBuilder
{
    public decimal Amount { get; private set; }
    public string? Description { get; private set; }
    public Guid? CategoryId { get; private set; }
    public Guid? AccountId { get; private set; }

    public UpdateTransactionRequestViewModelBuilder()
    {
        var faker = new Faker();

        WithAmount(faker.Finance.Amount());
        WithDescription(faker.Lorem.Paragraph());
        WithCategoryId(Guid.NewGuid());
        WithAccountId(Guid.NewGuid());
    }

    public UpdateTransactionRequestViewModelBuilder WithAmount(decimal amount)
    {
        Amount = amount;
        return this;
    }

    public UpdateTransactionRequestViewModelBuilder WithDescription(string? description)
    {
        Description = description;
        return this;
    }

    public UpdateTransactionRequestViewModelBuilder WithCategoryId(Guid? categoryId)
    {
        CategoryId = categoryId;
        return this;
    }

    public UpdateTransactionRequestViewModelBuilder WithAccountId(Guid? accountId)
    {
        AccountId = accountId;
        return this;
    }

    public static UpdateTransactionRequestViewModelBuilder New() => new();

    public UpdateTransactionRequestViewModel Build() => new(
        Amount,
        Description,
        CategoryId,
        AccountId
    );
}
