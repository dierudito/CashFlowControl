using AutoMapper;
using DMoreno.CashFlowControl.Application.AppServices;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Domain.Interfaces.Services;
using DMoreno.CashFlowControl.Domain.Interfaces.UoW;
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

    [Fact(DisplayName = "Should Return BadRequest When Adding Transaction The Request Conversion Fails")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.AddAsync))]
    public async Task ShouldReturnBadRequestWhenAddingTransactionTheRequestConversionFails()
    {
        // Arrange
        var request = AddTransactionRequestViewModelBuilder.New().Build();

        // Act
        var response = await appService.AddAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Should Not Persist Data When Adding Transaction The Request Conversion Fails")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.AddAsync))]
    public async Task ShouldNotPersistDataWhenAddingTransactionTheRequestConversionFails()
    {
        // Arrange
        var request = AddTransactionRequestViewModelBuilder.New().Build();

        // Act
        var response = await appService.AddAsync(request);

        // Assert
        transactionService.Verify(t => t.AddAsync(It.IsAny<Transaction>()), Times.Never());
    }

    [Fact(DisplayName = "Should Return UnprocessableEntity When Adding Transaction The SaveChangesAsync Returns False")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.AddAsync))]
    public async Task ShouldReturnUnprocessableEntityWhenAddingTransactionTheSaveChangesAsyncReturnsFalse()
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

    [Fact(DisplayName = "Should Return InternalServerError When Adding Transaction The Error Occurs While Persisting Data")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.AddAsync))]
    public async Task ShouldReturnInternalServerErrorWhenAddingTransactionTheErrorOccursWhilePersistingData()
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

    [Fact(DisplayName = "Should Return Transaction Correctly When Getting Transaction By Id")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.GetByIdAsync))]
    public async Task ShouldReturnTransactionCorrectlyGettingTransactionById()
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

    [Fact(DisplayName = "Should Return NotFound When Getting Transaction By Id Not Found The Transaction")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.GetByIdAsync))]
    public async Task ShouldReturnNotFoundWhenGettingTransactionByIdNotFoundTheTransaction()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await appService.GetByIdAsync(id);

        // Assert
        response.Code.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should Update Transaction Successfully")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.UpdateAsync))]
    public async Task ShouldUpdateTransactionSuccessfully()
    {
        // Arrange
        var idTransaction = Guid.NewGuid();
        var request = UpdateTransactionRequestViewModelBuilder.New().Build();
        var transaction = TransactionBuilder.New().Build();

        transactionRepository
            .Setup(t => t.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(transaction);
        mapper
            .Setup(m => m.Map<Transaction>(It.IsAny<UpdateTransactionRequestViewModel>()))
            .Returns(transaction);
        unitOfWork
            .Setup(u => u.CommitAsync())
            .ReturnsAsync(1);

        // Act
        var response = await appService.UpdateAsync(request, idTransaction);

        // Assert
        response.Code.Should().Be(HttpStatusCode.OK);
        transactionService.Verify(t => t.UpdateAsync(It.Is<Transaction>(entity =>
        entity.Id == transaction.Id &&
        entity.Type == transaction.Type &&
        entity.Amount == transaction.Amount &&
        entity.Date == transaction.Date)), Times.Once());

        unitOfWork.Verify(u => u.CommitAsync(), Times.Once());
    }

    [Fact(DisplayName = "Should Return NotFound When Updating Transaction Not Found The Transaction")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.UpdateAsync))]
    public async Task ShouldReturnNotFoundWhenUpdatingTransactionNotFoundTheTransaction()
    {
        // Arrange
        var idTransaction = Guid.NewGuid();
        var request = UpdateTransactionRequestViewModelBuilder.New().Build();

        // Act
        var response = await appService.UpdateAsync(request, idTransaction);

        // Assert
        response.Code.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should Return BadRequest When Updating Transaction The Request Conversion Fails")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.UpdateAsync))]
    public async Task ShouldReturnBadRequestWhenUpdatingTransactionTheRequestConversionFails()
    {
        // Arrange
        var idTransaction = Guid.NewGuid();
        var request = UpdateTransactionRequestViewModelBuilder.New().Build();
        var transaction = TransactionBuilder.New().Build();

        transactionRepository
            .Setup(t => t.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(transaction);

        // Act
        var response = await appService.UpdateAsync(request, idTransaction);

        // Assert
        response.Code.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Should Not Persist Data When Updating Transaction The Request Conversion Fails")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.UpdateAsync))]
    public async Task ShouldNotPersistDataWhenUpdatingTransactionTheRequestConversionFails()
    {
        // Arrange
        var idTransaction = Guid.NewGuid();
        var request = UpdateTransactionRequestViewModelBuilder.New().Build();
        var transaction = TransactionBuilder.New().Build();

        transactionRepository
            .Setup(t => t.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(transaction);

        // Act
        var response = await appService.UpdateAsync(request, idTransaction);

        // Assert
        transactionService.Verify(t => t.UpdateAsync(It.IsAny<Transaction>()), Times.Never());
    }

    [Fact(DisplayName = "Should Return UnprocessableEntity When Updating Transaction The SaveChangesAsync Returns False")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.UpdateAsync))]
    public async Task ShouldReturnUnprocessableEntityWhenUpdatingTransactionTheSaveChangesAsyncReturnsFalse()
    {
        // Arrange
        var idTransaction = Guid.NewGuid();
        var request = UpdateTransactionRequestViewModelBuilder.New().Build();
        var transaction = TransactionBuilder.New().Build();

        transactionRepository
            .Setup(t => t.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(transaction);
        mapper
            .Setup(m => m.Map<Transaction>(It.IsAny<UpdateTransactionRequestViewModel>()))
            .Returns(transaction);

        unitOfWork
            .Setup(u => u.CommitAsync())
            .ReturnsAsync(0);

        // Act
        var response = await appService.UpdateAsync(request, idTransaction);

        // Assert
        response.Code.Should().Be(HttpStatusCode.UnprocessableContent);
    }

    [Fact(DisplayName = "Should Return InternalServerError When Updating Transaction The Error Occurs While Persisting Data")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.UpdateAsync))]
    public async Task ShouldReturnInternalServerErrorWhenUpdatingTransactionTheErrorOccursWhilePersistingData()
    {
        // Arrange
        var idTransaction = Guid.NewGuid();
        var request = UpdateTransactionRequestViewModelBuilder.New().Build();
        var transaction = TransactionBuilder.New().Build();

        transactionRepository
            .Setup(t => t.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(transaction);
        mapper
            .Setup(m => m.Map<Transaction>(It.IsAny<UpdateTransactionRequestViewModel>()))
            .Returns(transaction);
        transactionService
            .Setup(u => u.UpdateAsync(It.IsAny<Transaction>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await appService.UpdateAsync(request, idTransaction);

        // Assert
        response.Code.Should().Be(HttpStatusCode.InternalServerError);
    }










    [Fact(DisplayName = "Should Delete Transaction Successfully")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.DeleteAsync))]
    public async Task ShouldDeleteTransactionSuccessfully()
    {
        // Arrange
        var idTransaction = Guid.NewGuid();
        var transaction = TransactionBuilder.New().Build();

        transactionRepository
            .Setup(t => t.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(transaction);
        unitOfWork
            .Setup(u => u.CommitAsync())
            .ReturnsAsync(1);

        // Act
        var response = await appService.DeleteAsync(idTransaction);

        // Assert
        response.Code.Should().Be(HttpStatusCode.OK);
        transactionService.Verify(t => t.DeleteAsync(idTransaction), Times.Once());

        unitOfWork.Verify(u => u.CommitAsync(), Times.Once());
    }

    [Fact(DisplayName = "Should Return NotFound When Deleting Transaction Not Found The Transaction")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.DeleteAsync))]
    public async Task ShouldReturnNotFoundWhenDeletingTransactionNotFoundTheTransaction()
    {
        // Arrange
        var idTransaction = Guid.NewGuid();

        // Act
        var response = await appService.DeleteAsync(idTransaction);

        // Assert
        response.Code.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should Return UnprocessableEntity When Deleting Transaction The SaveChangesAsync Returns False")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.DeleteAsync))]
    public async Task ShouldReturnUnprocessableEntityWhenDeletingTransactionTheSaveChangesAsyncReturnsFalse()
    {
        // Arrange
        var idTransaction = Guid.NewGuid();
        var transaction = TransactionBuilder.New().Build();

        transactionRepository
            .Setup(t => t.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(transaction);

        unitOfWork
            .Setup(u => u.CommitAsync())
            .ReturnsAsync(0);

        // Act
        var response = await appService.DeleteAsync(idTransaction);

        // Assert
        response.Code.Should().Be(HttpStatusCode.UnprocessableContent);
    }

    [Fact(DisplayName = "Should Return InternalServerError When Deleting Transaction The Error Occurs While Persisting Data")]
    [Trait(nameof(TransactionAppService), nameof(TransactionAppService.DeleteAsync))]
    public async Task ShouldReturnInternalServerErrorWhenDeletingTransactionTheErrorOccursWhilePersistingData()
    {
        // Arrange
        var idTransaction = Guid.NewGuid();
        var transaction = TransactionBuilder.New().Build();

        transactionRepository
            .Setup(t => t.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(transaction);
        transactionService
            .Setup(u => u.DeleteAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await appService.DeleteAsync(idTransaction);

        // Assert
        response.Code.Should().Be(HttpStatusCode.InternalServerError);
    }
}