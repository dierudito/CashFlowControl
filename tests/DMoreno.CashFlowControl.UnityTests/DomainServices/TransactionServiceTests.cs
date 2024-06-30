using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Domain.Services;
using DMoreno.CashFlowControl.UnityTests.Shared.Builders;
using FluentAssertions;
using Moq;
using Moq.AutoMock;

namespace DMoreno.CashFlowControl.UnityTests.DomainServices;
public class TransactionServiceTests
{
    private readonly Mock<ITransactionRepository> transactionRepository;
    private readonly TransactionService service;

    public TransactionServiceTests()
    {
        var mocker = new AutoMocker();
        transactionRepository = mocker.GetMock<ITransactionRepository>();
        service = mocker.CreateInstance<TransactionService>();
    }

    [Fact(DisplayName = "Should Add Transaction Successfully")]
    [Trait(nameof(TransactionService), nameof(TransactionService.AddAsync))]
    public async Task ShouldAddTransactionSuccessfully()
    {
        // Arrange
        var transaction = TransactionBuilder.New().Build();

        transactionRepository
            .Setup(t => t.AddAsync(It.IsAny<Transaction>()))
            .ReturnsAsync(transaction);

        // Act
        var response = await service.AddAsync(transaction);

        // Assert
        response.Should().BeEquivalentTo(transaction);
        transactionRepository.Verify(t => t.AddAsync(It.Is<Transaction>(entity =>
        entity.Id == transaction.Id &&
        entity.Type == transaction.Type &&
        entity.Amount == transaction.Amount &&
        entity.Date == transaction.Date)), Times.Once());
    }

    [Fact(DisplayName = "Should Update Transaction Successfully")]
    [Trait(nameof(TransactionService), nameof(TransactionService.UpdateAsync))]
    public async Task ShouldUpdateTransactionSuccessfully()
    {
        // Arrange
        var transaction = TransactionBuilder.New().Build();
        var idTransaction = Guid.NewGuid();

        transactionRepository
            .Setup(t => t.UpdateAsync(It.IsAny<Transaction>(), It.IsAny<Guid>()))
            .ReturnsAsync(transaction);

        // Act
        var response = await service.UpdateAsync(transaction, idTransaction);

        // Assert
        response.Should().BeEquivalentTo(transaction);
        transactionRepository.Verify(t => t.UpdateAsync(It.Is<Transaction>(entity =>
        entity.Id == transaction.Id &&
        entity.Type == transaction.Type &&
        entity.Amount == transaction.Amount &&
        entity.Date == transaction.Date), idTransaction), Times.Once());
    }

    [Fact(DisplayName = "Should Delete Transaction Successfully")]
    [Trait(nameof(TransactionService), nameof(TransactionService.DeleteAsync))]
    public async Task ShouldDeleteTransactionSuccessfully()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await service.DeleteAsync(id);

        // Assert
        transactionRepository.Verify(t => t.DeleteAsync(id), Times.Once());
    }
}