using AutoMapper;
using Bogus;
using DMoreno.CashFlowControl.Application.AppServices;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Domain.Interfaces.Services;
using DMoreno.CashFlowControl.Domain.Interfaces.UoW;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System.Linq.Expressions;
using System.Net;

namespace DMoreno.CashFlowControl.UnityTests.AppServices;

public class AccountAppServiceTests
{
    private readonly Faker faker;
    private readonly Mock<IMapper> mapper;
    private readonly Mock<IAccountService> accountService;
    private readonly Mock<IAccountRepository> accountRepository;
    private readonly Mock<ITransactionRepository> transactionRepository;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly AccountAppService appService;

    public AccountAppServiceTests()
    {
        var mocker = new AutoMocker();
        faker = new();

        mapper = mocker.GetMock<IMapper>();
        accountService = mocker.GetMock<IAccountService>();
        accountRepository = mocker.GetMock<IAccountRepository>();
        transactionRepository = mocker.GetMock<ITransactionRepository>();
        unitOfWork = mocker.GetMock<IUnitOfWork>();

        appService = mocker.CreateInstance<AccountAppService>();
    }

    [Fact(DisplayName = "Should Add Account Successfully")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.AddAsync))]
    public async Task ShouldAddAccountSuccessfully()
    {
        // Arrange
        var request = new AccountRequestViewModel(faker.Lorem.Word(), faker.Lorem.Text());
        var account = new Account { Description = request.Description, Name = request.Name };
        var accountResponse = new AccountResponseViewModel(account.Id, account.Name, account.Description);

        mapper
            .Setup(m => m.Map<Account>(It.IsAny<AccountRequestViewModel>()))
            .Returns(account);
        mapper
            .Setup(m => m.Map<AccountResponseViewModel>(It.IsAny<Account>()))
            .Returns(accountResponse);

        // Act
        var response = await appService.AddAsync(request);

        // Assert
        response.Data.Should().BeEquivalentTo(accountResponse);
        response.Code.Should().Be(HttpStatusCode.OK);
        accountService.Verify(t => t.AddAsync(It.Is<Account>(entity =>
        entity.Id == account.Id &&
        entity.Name == request.Name &&
        entity.Description == request.Description)), Times.Once());

        unitOfWork.Verify(u => u.CommitAsync(), Times.Once());
    }

    [Fact(DisplayName = "Should Return BadRequest When Adding Account The Request Conversion Fails")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.AddAsync))]
    public async Task ShouldReturnBadRequestWhenAddingAccountTheRequestConversionFails()
    {
        // Arrange
        var request = new AccountRequestViewModel(faker.Lorem.Word(), faker.Lorem.Text());

        // Act
        var response = await appService.AddAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Should Not Persist Data When Adding Account The Request Conversion Fails")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.AddAsync))]
    public async Task ShouldNotPersistDataWhenAddingAccountTheRequestConversionFails()
    {
        // Arrange
        var request = new AccountRequestViewModel(faker.Lorem.Word(), faker.Lorem.Text());

        // Act
        var response = await appService.AddAsync(request);

        // Assert
        accountService.Verify(t => t.AddAsync(It.IsAny<Account>()), Times.Never());
    }

    [Fact(DisplayName = "Should Return InternalServerError When Adding Account The Error Occurs While Persisting Data")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.AddAsync))]
    public async Task ShouldReturnInternalServerErrorWhenAddingAccountTheErrorOccursWhilePersistingData()
    {
        // Arrange
        var request = new AccountRequestViewModel(faker.Lorem.Word(), faker.Lorem.Text());
        var account = new Account { Description = faker.Lorem.Text(), Name = faker.Lorem.Word() };

        mapper
            .Setup(m => m.Map<Account>(It.IsAny<AccountRequestViewModel>()))
            .Returns(account);
        accountService
            .Setup(u => u.AddAsync(It.IsAny<Account>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await appService.AddAsync(request);

        // Assert
        response.Code.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact(DisplayName = "Should Return Account Correctly When Getting Account By Id")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.GetByIdAsync))]
    public async Task ShouldReturnAccountCorrectlyGettingAccountById()
    {
        // Arrange
        var id = Guid.NewGuid();
        var account = new Account { Description = faker.Lorem.Text(), Name = faker.Lorem.Word() };
        var accountResponse = new AccountResponseViewModel(account.Id, account.Name, account.Description);

        accountRepository
            .Setup(t => t.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(account);

        mapper
            .Setup(m => m.Map<AccountResponseViewModel>(It.IsAny<Account>()))
            .Returns(accountResponse);

        // Act
        var response = await appService.GetByIdAsync(id);

        // Assert
        response.Code.Should().Be(HttpStatusCode.OK);
        response.Data.Should().BeEquivalentTo(accountResponse);
    }

    [Fact(DisplayName = "Should Return NotFound When Getting Account By Id Not Found The Account")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.GetByIdAsync))]
    public async Task ShouldReturnNotFoundWhenGettingAccountByIdNotFoundTheAccount()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await appService.GetByIdAsync(id);

        // Assert
        response.Code.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should Update Account Successfully")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.UpdateAsync))]
    public async Task ShouldUpdateAccountSuccessfully()
    {
        // Arrange
        var idAccount = Guid.NewGuid();

        var request = new AccountRequestViewModel(faker.Lorem.Word(), faker.Lorem.Text());
        var account = new Account { Description = request.Description, Name = request.Name, Id = idAccount };

        accountService
            .Setup(t => t.UpdateAsync(It.IsAny<Account>(), It.IsAny<Guid>()))
            .ReturnsAsync(account);
        mapper
            .Setup(m => m.Map<Account>(It.IsAny<AccountRequestViewModel>()))
            .Returns(account);

        // Act
        var response = await appService.UpdateAsync(request, idAccount);

        // Assert
        response.Code.Should().Be(HttpStatusCode.OK);
        accountService.Verify(t => t.UpdateAsync(It.Is<Account>(entity =>
        entity.Id == idAccount &&
        entity.Name == request.Name &&
        entity.Description == request.Description), idAccount), Times.Once());

        unitOfWork.Verify(u => u.CommitAsync(), Times.Once());
    }

    [Fact(DisplayName = "Should Return NotFound When Updating Account Not Found The Account")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.UpdateAsync))]
    public async Task ShouldReturnNotFoundWhenUpdatingAccountNotFoundTheAccount()
    {
        // Arrange
        var idAccount = Guid.NewGuid();
        var request = new AccountRequestViewModel(faker.Lorem.Word(), faker.Lorem.Text());
        var account = new Account { Description = request.Description, Name = request.Name, Id = idAccount };

        mapper
            .Setup(m => m.Map<Account>(It.IsAny<AccountRequestViewModel>()))
            .Returns(account);

        // Act
        var response = await appService.UpdateAsync(request, idAccount);

        // Assert
        response.Code.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should Return BadRequest When Updating Account The Request Conversion Fails")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.UpdateAsync))]
    public async Task ShouldReturnBadRequestWhenUpdatingAccountTheRequestConversionFails()
    {
        // Arrange
        var idAccount = Guid.NewGuid();
        var request = new AccountRequestViewModel(faker.Lorem.Word(), faker.Lorem.Text());
        var account = new Account { Description = request.Description, Name = request.Name, Id = idAccount };

        // Act
        var response = await appService.UpdateAsync(request, idAccount);

        // Assert
        response.Code.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Should Not Persist Data When Updating Account The Request Conversion Fails")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.UpdateAsync))]
    public async Task ShouldNotPersistDataWhenUpdatingAccountTheRequestConversionFails()
    {
        // Arrange
        var idAccount = Guid.NewGuid();
        var request = new AccountRequestViewModel(faker.Lorem.Word(), faker.Lorem.Text());
        var account = new Account { Description = request.Description, Name = request.Name, Id = idAccount };

        // Act
        var response = await appService.UpdateAsync(request, idAccount);

        // Assert
        accountService.Verify(t => t.UpdateAsync(It.IsAny<Account>(), It.IsAny<Guid>()), Times.Never());
    }

    [Fact(DisplayName = "Should Return InternalServerError When Updating Account The Error Occurs While Persisting Data")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.UpdateAsync))]
    public async Task ShouldReturnInternalServerErrorWhenUpdatingAccountTheErrorOccursWhilePersistingData()
    {
        // Arrange
        var idAccount = Guid.NewGuid();
        var request = new AccountRequestViewModel(faker.Lorem.Word(), faker.Lorem.Text());
        var account = new Account { Description = request.Description, Name = request.Name, Id = idAccount };

        mapper
            .Setup(m => m.Map<Account>(It.IsAny<AccountRequestViewModel>()))
            .Returns(account);
        accountService
            .Setup(u => u.UpdateAsync(It.IsAny<Account>(), It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await appService.UpdateAsync(request, idAccount);

        // Assert
        response.Code.Should().Be(HttpStatusCode.InternalServerError);
    }










    [Fact(DisplayName = "Should Delete Account Successfully")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.DeleteAsync))]
    public async Task ShouldDeleteAccountSuccessfully()
    {
        // Arrange
        var idAccount = Guid.NewGuid();

        // Act
        var response = await appService.DeleteAsync(idAccount);

        // Assert
        response.Code.Should().Be(HttpStatusCode.OK);
        accountService.Verify(t => t.DeleteAsync(idAccount), Times.Once());

        unitOfWork.Verify(u => u.CommitAsync(), Times.Once());
    }

    [Fact(DisplayName = "Should Return InternalServerError When Deleting Account The Error Occurs While Persisting Data")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.DeleteAsync))]
    public async Task ShouldReturnInternalServerErrorWhenDeletingAccountTheErrorOccursWhilePersistingData()
    {
        // Arrange
        var idAccount = Guid.NewGuid();
        var account = new Account { Description = faker.Lorem.Text(), Name = faker.Lorem.Word() };

        accountRepository
            .Setup(t => t.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(account);
        accountService
            .Setup(u => u.DeleteAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await appService.DeleteAsync(idAccount);

        // Assert
        response.Code.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact(DisplayName = "Should Return NotAcceptable When Attempting To Delete a Account That Is Currently Assigned To a Transaction")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.DeleteAsync))]
    public async Task ShouldReturnNotAcceptableWhenAttemptingToDeleteAccountThatIsCurrentlyAssignedToTransaction()
    {
        // Arrange
        var idAccount = Guid.NewGuid();

        transactionRepository
            .Setup(t => t.AreThereAsync(It.IsAny<Expression<Func<Transaction, bool>>>()))
            .ReturnsAsync(true);

        // Act
        var response = await appService.DeleteAsync(idAccount);

        // Assert
        response.Code.Should().Be(HttpStatusCode.NotAcceptable);
    }

    [Fact(DisplayName = "Should Not Persist Data When Attempting To Delete a Account That Is Currently Assigned To a Transaction")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.DeleteAsync))]
    public async Task ShouldNotPersistDataWhenAttemptingToDeleteAccountThatIsCurrentlyAssignedToTransaction()
    {
        // Arrange
        var idAccount = Guid.NewGuid();

        transactionRepository
            .Setup(t => t.AreThereAsync(It.IsAny<Expression<Func<Transaction, bool>>>()))
            .ReturnsAsync(true);

        // Act
        var response = await appService.DeleteAsync(idAccount);

        // Assert
        accountService.Verify(t => t.DeleteAsync(It.IsAny<Guid>()), Times.Never());

        unitOfWork.Verify(u => u.CommitAsync(), Times.Never());
    }

    [Fact(DisplayName = "Should Get All Accounts Currently")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.GetAllAsync))]
    public async Task ShouldGetAllAccountsCurrently()
    {
        // Arrange
        var account = new Account { Description = faker.Lorem.Text(), Name = faker.Lorem.Word() };
        var accountResponse = new AccountResponseViewModel(account.Id, account.Name, account.Description);
        List<AccountResponseViewModel> accountsResponse = [accountResponse];

        accountRepository
            .Setup(c => c.GetAllAsync())
            .ReturnsAsync([account]);

        mapper
            .Setup(m => m.Map<List<AccountResponseViewModel>>(It.IsAny<IEnumerable<Account>>()))
            .Returns(accountsResponse);

        // Act
        var response = await appService.GetAllAsync();

        // Assert
        response.Data.Should().HaveCount(accountsResponse.Count);
        response.Data.Should().BeEquivalentTo(accountsResponse);
    }

    [Fact(DisplayName = "Should Return NoContent Get All Accounts When There Aren't Accounts")]
    [Trait(nameof(AccountAppService), nameof(AccountAppService.GetAllAsync))]
    public async Task ShouldReturnNoContentGetAllAccountsWhenThereAreNotAccounts()
    {
        // Arrange

        // Act
        var response = await appService.GetAllAsync();

        // Assert
        response.Code.Should().Be(HttpStatusCode.NoContent);
    }
}