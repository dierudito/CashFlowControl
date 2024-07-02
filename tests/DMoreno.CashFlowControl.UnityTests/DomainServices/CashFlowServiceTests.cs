using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Extensions;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Domain.Services;
using DMoreno.CashFlowControl.UnityTests.Shared.Builders;
using FluentAssertions;
using Moq;
using Moq.AutoMock;

namespace DMoreno.CashFlowControl.UnityTests.DomainServices;

public class CashFlowServiceTests
{
    private readonly Mock<ICashFlowRepository> cashflowRepository;
    private readonly CashFlowService service;

    public CashFlowServiceTests()
    {
        var mocker = new AutoMocker();
        cashflowRepository = mocker.GetMock<ICashFlowRepository>();
        service = mocker.CreateInstance<CashFlowService>();
    }

    [Fact(DisplayName = "Should Add CashFlow Successfully")]
    [Trait(nameof(CashFlowService), nameof(CashFlowService.AddAsync))]
    public async Task ShouldAddCashFlowSuccessfully()
    {
        // Arrange
        var cashflow = CashFlowBuilder.New().Build();

        cashflowRepository
            .Setup(t => t.AddAsync(It.IsAny<CashFlow>()))
            .ReturnsAsync(cashflow);

        // Act
        var response = await service.AddAsync(cashflow);

        // Assert
        response.Should().BeEquivalentTo(cashflow);
        cashflowRepository.Verify(t => t.AddAsync(It.Is<CashFlow>(entity =>
        entity.Id == cashflow.Id &&
        entity.ReleaseDate == cashflow.ReleaseDate &&
        entity.TotalDebits == cashflow.TotalDebits &&
        entity.TotalCredits == cashflow.TotalCredits &&
        entity.OpeningBalance == cashflow.OpeningBalance)), Times.Once());
    }

    [Fact(DisplayName = "Should Update CashFlow Successfully")]
    [Trait(nameof(CashFlowService), nameof(CashFlowService.UpdateAsync))]
    public async Task ShouldUpdateCashFlowSuccessfully()
    {
        // Arrange
        var cashflow = CashFlowBuilder.New().Build();
        var idCashFlow = Guid.NewGuid();

        cashflowRepository
            .Setup(t => t.UpdateAsync(It.IsAny<CashFlow>(), It.IsAny<Guid>()))
            .ReturnsAsync(cashflow);

        // Act
        var response = await service.UpdateAsync(cashflow, idCashFlow);

        // Assert
        response.Should().BeEquivalentTo(cashflow);
        cashflowRepository.Verify(t => t.UpdateAsync(It.Is<CashFlow>(entity =>
        entity.Id == cashflow.Id &&
        entity.ReleaseDate == cashflow.ReleaseDate &&
        entity.TotalDebits == cashflow.TotalDebits &&
        entity.TotalCredits == cashflow.TotalCredits &&
        entity.OpeningBalance == cashflow.OpeningBalance), idCashFlow), Times.Once());
    }

    [Fact(DisplayName = "Should Delete CashFlow Successfully")]
    [Trait(nameof(CashFlowService), nameof(CashFlowService.DeleteAsync))]
    public async Task ShouldDeleteCashFlowSuccessfully()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await service.DeleteAsync(id);

        // Assert
        cashflowRepository.Verify(t => t.DeleteAsync(id), Times.Once());
    }

    [Fact(DisplayName = "Should Return a Existing The Same Cache Flow When Add With Previous Balance")]
    [Trait(nameof(CashFlowService), nameof(CashFlowService.AddWithPreviousBalanceAsync))]
    public async Task ShouldReturnExistingTheSameCacheFlowWhenAddWithPreviousBalance()
    {
        // Arrange
        var cashFlow = CashFlowBuilder.New().WithReleaseDate(DateTime.Now.DateOnly()).Build();
        cashflowRepository
            .Setup(c => c.GetLatestAsync())
            .ReturnsAsync(cashFlow);

        // Act
        var response = await service.AddWithPreviousBalanceAsync();

        // Assert
        response.Should().BeEquivalentTo(cashFlow);
        cashflowRepository
            .Verify(c => c.AddAsync(It.IsAny<CashFlow>()), Times.Never);
    }

    [Fact(DisplayName = "Should Add a new Cache Flow With OpeningBalance Zero When Add With Previous Balance There Aren't Previous Balance")]
    [Trait(nameof(CashFlowService), nameof(CashFlowService.AddWithPreviousBalanceAsync))]
    public async Task ShouldAddNewCacheFlowWithOpeningBalanceZeroWhenAddWithPreviousBalanceThereAreNotPreviousBalance()
    {
        // Arrange

        // Act
        await service.AddWithPreviousBalanceAsync();

        // Assert
        cashflowRepository
            .Verify(c => c.AddAsync(It.Is<CashFlow>(e => e.OpeningBalance == 0)), Times.Once);
    }

    [Fact(DisplayName = "Should Add a new Cache Flow When Add With Previous Balance")]
    [Trait(nameof(CashFlowService), nameof(CashFlowService.AddWithPreviousBalanceAsync))]
    public async Task ShouldAddNewCacheFlowWhenAddWithPreviousBalance()
    {
        // Arrange
        var cashFlow = CashFlowBuilder.New().WithReleaseDate(DateTime.Now.AddDays(-1).DateOnly()).Build();
        cashflowRepository
            .Setup(c => c.GetLatestAsync())
            .ReturnsAsync(cashFlow);

        // Act
        await service.AddWithPreviousBalanceAsync();

        // Assert
        cashflowRepository
            .Verify(c => c.AddAsync(It.Is<CashFlow>(e =>
            e.OpeningBalance == cashFlow.ClosingBalance)), Times.Once);
    }

    [Fact(DisplayName = "Should Get a Existing CashFlow When Get Or Create By Date There Is Corresponding CashFlow")]
    [Trait(nameof(CashFlowService), nameof(CashFlowService.GetOrCreateByDateAsync))]
    public async Task ShouldGetExistingCashFlowWhenGetOrCreateByDateThereIsCorrespondingCashFlow()
    {
        // Arrange
        var date = DateTime.Now.DateOnly();
        var cashFlow = CashFlowBuilder.New().WithReleaseDate(date).Build();

        cashflowRepository
            .Setup(c => c.GetByDateAsync(It.IsAny<DateOnly>()))
            .ReturnsAsync(cashFlow);

        // Act
        var response = await service.GetOrCreateByDateAsync(date);

        response.Should().BeEquivalentTo(cashFlow);
    }
}