using AutoMapper;
using DMoreno.CashFlowControl.Application.AppServices;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Domain.Interfaces.Services;
using DMoreno.CashFlowControl.Domain.Interfaces.UoW;
using DMoreno.CashFlowControl.UnityTests.Shared;
using DMoreno.CashFlowControl.UnityTests.Shared.Builders;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System.Net;

namespace DMoreno.CashFlowControl.UnityTests.AppServices;
public class TransactionAppServiceTests
{
    private readonly Mock<IMapper> mapper;
    private readonly Mock<ITransactionService> transactionService;
    private readonly Mock<ITransactionRepository> transactionRepository;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly TransactionAppService appService;

    public TransactionAppServiceTests()
    {
        var mocker = new AutoMocker();

        mapper = mocker.GetMock<IMapper>();
        transactionService = mocker.GetMock<ITransactionService>();
        transactionRepository = mocker.GetMock<ITransactionRepository>();
        unitOfWork = mocker.GetMock<IUnitOfWork>();

        appService = mocker.CreateInstance<TransactionAppService>();
    }

    [Fact(DisplayName = "Should Add Transaction Successfully")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.AddAsync))]
    public async Task ShouldAddTransactionSuccessfully()
    {
        // Arrange
        var request = AddTransactionRequestViewModelBuilder.New().Build();
        var transaction = TransactionBuilder.New().Build();
        var transcationResponse = AddTransactionResponseViewModelBuilder.New().Build();

        mapper
            .Setup(m => m.Map<Transaction>(It.IsAny<AddTransactionRequestViewModel>()))
            .Returns(transaction);
        mapper
            .Setup(m => m.Map<AddTransactionResponseViewModel>(It.IsAny<Transaction>()))
            .Returns(transcationResponse);
        unitOfWork
            .Setup(u => u.CommitAsync())
            .ReturnsAsync(1);

        // Act
        var response = await appService.AddAsync(request);

        // Assert
        response.Data.Should().BeEquivalentTo(transcationResponse);
        response.Code.Should().Be(HttpStatusCode.OK);
        transactionService.Verify(t => t.AddAsync(It.Is<Transaction>(entity => 
        entity.Id == transaction.Id &&
        entity.Type == transaction.Type &&
        entity.Amount == transaction.Amount &&
        entity.Date == transaction.Date)), Times.Once());

        unitOfWork.Verify(u => u.CommitAsync(), Times.Once());
    }

    [Fact(DisplayName = "Should Return BadRequest When Request Conversion Fails")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.AddAsync))]
    public async Task ShouldReturnBadRequestWhenRequestConversionFails()
    {
        // Arrange
        var request = AddTransactionRequestViewModelBuilder.New().Build();

        // Act
        var response = await appService.AddAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Should Not Persist Data When Request Conversion Fails")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.AddAsync))]
    public async Task ShouldNotPersistDataWhenRequestConversionFails()
    {
        // Arrange
        var request = AddTransactionRequestViewModelBuilder.New().Build();

        // Act
        var response = await appService.AddAsync(request);

        // Assert
        transactionService.Verify(t => t.AddAsync(It.IsAny<Transaction>()), Times.Never());
    }

    [Fact(DisplayName = "Should Return UnprocessableEntity When SaveChangesAsync Returns False")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.AddAsync))]
    public async Task ShouldReturnUnprocessableEntityWhenSaveChangesAsyncReturnsFalse()
    {
        // Arrange
        var request = AddTransactionRequestViewModelBuilder.New().Build();
        var transaction = TransactionBuilder.New().Build();

        mapper
            .Setup(m => m.Map<Transaction>(It.IsAny<AddTransactionRequestViewModel>()))
            .Returns(transaction);

        unitOfWork
            .Setup(u => u.CommitAsync())
            .ReturnsAsync(0);

        // Act
        var response = await appService.AddAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.UnprocessableContent);
    }

    [Fact(DisplayName = "Should Return InternalServerError When Error Occurs While Persisting Data")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.AddAsync))]
    public async Task ShouldReturnInternalServerErrorWhenErrorOccursWhilePersistingData()
    {
        // Arrange
        var request = AddTransactionRequestViewModelBuilder.New().Build();
        var transaction = TransactionBuilder.New().Build();

        mapper
            .Setup(m => m.Map<Transaction>(It.IsAny<AddTransactionRequestViewModel>()))
            .Returns(transaction);
        transactionService
            .Setup(u => u.AddAsync(It.IsAny<Transaction>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await appService.AddAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact(DisplayName = "Should Return BadRequest When There isn't Transaction")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.GetByIdAsync))]
    public async Task ShouldReturnBadRequestWhenThereIsNotTransaction()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await appService.GetByIdAsync(id);

        // Assert
        response.Code.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Should Return Transaction Correctly")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.GetByIdAsync))]
    public async Task ShouldReturnTransactionCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var transaction = TransactionBuilder.New().Build();
        var transactionResponse = GetTransactionByIdResponseViewModelBuilder.New().Build();

        transactionRepository
            .Setup(t => t.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(transaction);

        mapper
            .Setup(m => m.Map<GetTransactionByIdResponseViewModel>(It.IsAny<Transaction>()))
            .Returns(transactionResponse);

        // Act
        var response = await appService.GetByIdAsync(id);

        // Assert
        response.Code.Should().Be(HttpStatusCode.OK);
        response.Data.Should().BeEquivalentTo(transactionResponse);
    }
}