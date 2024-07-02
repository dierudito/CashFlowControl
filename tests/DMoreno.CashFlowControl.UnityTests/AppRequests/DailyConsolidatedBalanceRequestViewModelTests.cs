using Bogus;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Domain.Extensions;
using FluentAssertions;

namespace DMoreno.CashFlowControl.UnityTests.AppRequests;
public class DailyConsolidatedBalanceRequestViewModelTests
{
    [Fact(DisplayName = "Should Create DailyConsolidatedBalanceRequestViewModel")]
    [Trait(nameof(DailyConsolidatedBalanceRequestViewModel), nameof(DailyConsolidatedBalanceRequestViewModel))]
    public void ShouldCreateDailyConsolidatedBalanceRequestViewModel()
    {
        // Arrange
        var faker = new Faker();
        var startDate = faker.Date.Recent();
        var endDate = faker.Date.Recent();

        // Act
        var requestViewModel = new DailyConsolidatedBalanceRequestViewModel(startDate, endDate);

        // Assert
        requestViewModel.StartDate.Should().Be(startDate.DateOnly());
        requestViewModel.EndDate.Should().Be(endDate.DateOnly());
    }
}
