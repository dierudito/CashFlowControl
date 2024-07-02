using Bogus;
using DMoreno.CashFlowControl.Domain.Entities;

namespace DMoreno.CashFlowControl.UnityTests.Shared.Builders;
public class CashFlowBuilder
{
    public Guid Id { get; private set; }
    public DateOnly ReleaseDate { get; private set; }
    public decimal OpeningBalance { get; private set; }
    public decimal TotalCredits { get; private set; }
    public decimal TotalDebits { get; private set; }

    public CashFlowBuilder()
    {
        var faker = new Faker();

        WithId(Guid.NewGuid());
        WithReleaseDate(DateOnly.FromDateTime(faker.Date.Recent()));
        WithOpeningBalance(faker.Finance.Amount());
        WithTotalCredits(faker.Finance.Amount());
        WithTotalDebits(faker.Finance.Amount());
    }

    public CashFlowBuilder WithId(Guid id)
    {
        Id = id;
        return this;
    }

    public CashFlowBuilder WithReleaseDate(DateOnly releaseDate)
    {
        ReleaseDate = releaseDate;
        return this;
    }

    public CashFlowBuilder WithOpeningBalance(decimal openingBalance)
    {
        OpeningBalance = openingBalance;
        return this;
    }

    public CashFlowBuilder WithTotalCredits(decimal totalCredits)
    {
        TotalCredits = totalCredits;
        return this;
    }

    public CashFlowBuilder WithTotalDebits(decimal totalDebits)
    {
        TotalDebits = totalDebits;
        return this;
    }

    public static CashFlowBuilder New() => new();

    public CashFlow Build()
    {
        CashFlow cashFlow = new()
        {
            Id = Id,
            ReleaseDate = ReleaseDate,
            OpeningBalance = OpeningBalance,
            TotalCredits = TotalCredits,
            TotalDebits = TotalDebits
        };

        cashFlow.SetClosingBalance();

        return cashFlow;
    }
}
