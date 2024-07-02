using Bogus;
using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Enums;
using DMoreno.CashFlowControl.UnityTests.Shared.Builders;
using FluentAssertions;

namespace DMoreno.CashFlowControl.UnityTests.Domain;
public class CashFlowTests
{
    private readonly Faker faker = new();

    [Fact(DisplayName = "Should Set Closing Balance Correctly")]
    [Trait(nameof(CashFlow), nameof(CashFlow.SetClosingBalance))]
    public void ShouldSetClosingBalanceCorrectly()
    {
        // Arrange
        var cashFlow = CashFlowBuilder.New().Build();
        var valueExpected = cashFlow.OpeningBalance + cashFlow.TotalCredits - cashFlow.TotalDebits;

        // Act
        cashFlow.SetClosingBalance();

        // Assert
        cashFlow.ClosingBalance.Should().Be(valueExpected);
    }

    [Fact(DisplayName = "Should Increment Total Debits")]
    [Trait(nameof(CashFlow), nameof(CashFlow.IncrementTotals))]
    public void ShouldIncrementTotalDebits()
    {
        // Arrange
        var cashFlow = CashFlowBuilder.New().Build();
        var amount = faker.Finance.Amount();
        var totalDebits = cashFlow.TotalDebits + amount;

        // Act
        cashFlow.IncrementTotals(ETransactionType.debit, amount);

        // Assert
        cashFlow.TotalDebits.Should().Be(totalDebits);
    }

    [Fact(DisplayName = "Should Increment Total Credits")]
    [Trait(nameof(CashFlow), nameof(CashFlow.IncrementTotals))]
    public void ShouldIncrementTotalCredits()
    {
        // Arrange
        var cashFlow = CashFlowBuilder.New().Build();
        var amount = faker.Finance.Amount();
        var totalCredits = cashFlow.TotalCredits + amount;

        // Act
        cashFlow.IncrementTotals(ETransactionType.credit, amount);

        // Assert
        cashFlow.TotalCredits.Should().Be(totalCredits);
    }
}
