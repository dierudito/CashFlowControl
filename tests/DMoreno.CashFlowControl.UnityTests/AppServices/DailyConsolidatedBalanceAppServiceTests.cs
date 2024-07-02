using AutoMapper;
using DMoreno.CashFlowControl.Application.AppServices;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Infra.CrossCutting.Shared;
using DMoreno.CashFlowControl.UnityTests.Shared.Builders;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System.Net;

namespace DMoreno.CashFlowControl.UnityTests.AppServices;
public class DailyConsolidatedBalanceAppServiceTests
{
    private readonly Mock<ICashFlowRepository> cashFlowRepository;
    private readonly Mock<IMapper> mapper;
    private readonly DailyConsolidatedBalanceAppService appService;

    public DailyConsolidatedBalanceAppServiceTests()
    {
        var mocker = new AutoMocker();

        cashFlowRepository = mocker.GetMock<ICashFlowRepository>();
        mapper = mocker.GetMock<IMapper>();

        appService = mocker.CreateInstance<DailyConsolidatedBalanceAppService>();
    }

    [Fact(DisplayName = "Should Return Daily Consolidated Correctly")]
    [Trait(nameof(DailyConsolidatedBalanceAppService), nameof(DailyConsolidatedBalanceAppService.GetByPeriodAsync))]
    public async Task ShouldReturnDailyConsolidatedCorrectly()
    {
        // Arrange
        var request =
            new DailyConsolidatedBalanceRequestViewModel(DateTime.Now.AddDays(ApiConfigurations.MaxLengthPeriodDays * -1), DateTime.Now);
        var dailyConsolidated = CashFlowBuilder.New().Build();
        var dailyResponse = new DailyConsolidatedBalanceResponseViewModel(
            dailyConsolidated.ReleaseDate.ToString("dd/MM/yyyy"),
            dailyConsolidated.OpeningBalance.ToString("C2"),
            dailyConsolidated.TotalCredits.ToString("C2"),
            dailyConsolidated.TotalDebits.ToString("C2"),
            dailyConsolidated.ClosingBalance.ToString("C2"));

        cashFlowRepository
            .Setup(c => c.GetByPeriodAsync(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ReturnsAsync([dailyConsolidated]);

        mapper
            .Setup(m => m.Map<List<DailyConsolidatedBalanceResponseViewModel>>(It.IsAny<List<CashFlow>>()))
            .Returns([dailyResponse]);

        // Act
        var response = await appService.GetByPeriodAsync(request);

        // Assert
        response.Data.Should().HaveCount(1);
        response.Data.Should().BeEquivalentTo([dailyResponse]);
    }

    [Fact(DisplayName = "Should Return BadRequest When Period Size Is Greater Than Max Length")]
    [Trait(nameof(DailyConsolidatedBalanceAppService), nameof(DailyConsolidatedBalanceAppService.GetByPeriodAsync))]
    public async Task ShouldReturnBadRequestWhenPeriodSizeIsGreaterThanMaxLength()
    {
        // Arrange
        var request =
            new DailyConsolidatedBalanceRequestViewModel(DateTime.Now.AddDays(ApiConfigurations.MaxLengthPeriodDays * -1), DateTime.Now.AddDays(1));

        // Act
        var response = await appService.GetByPeriodAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Should Return NoContent When There Arent Transaction In Thar Period")]
    [Trait(nameof(DailyConsolidatedBalanceAppService), nameof(DailyConsolidatedBalanceAppService.GetByPeriodAsync))]
    public async Task ShouldReturnNoContentWhenThereArentTransactionInThatPeriod()
    {
        // Arrange
        var request =
            new DailyConsolidatedBalanceRequestViewModel(DateTime.Now.AddDays(ApiConfigurations.MaxLengthPeriodDays * -1), DateTime.Now);

        cashFlowRepository
            .Setup(c => c.GetByPeriodAsync(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ReturnsAsync([]);

        // Act
        var response = await appService.GetByPeriodAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.NoContent);
    }
}
