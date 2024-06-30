using Bogus;
using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Domain.Services;
using FluentAssertions;
using Moq;
using Moq.AutoMock;

namespace DMoreno.CashFlowControl.UnityTests.DomainServices;

public class AccountServiceTests
{
    private readonly Faker faker;
    private readonly Mock<IAccountRepository> accountRepository;
    private readonly AccountService service;

    public AccountServiceTests()
    {
        faker = new();
        var mocker = new AutoMocker();
        accountRepository = mocker.GetMock<IAccountRepository>();
        service = mocker.CreateInstance<AccountService>();
    }

    [Fact(DisplayName = "Should Add Account Successfully")]
    [Trait(nameof(AccountService), nameof(AccountService.AddAsync))]
    public async Task ShouldAddAccountSuccessfully()
    {
        // Arrange
        var account = new Account { Name = faker.Lorem.Word(), Description = faker.Lorem.Word() };

        accountRepository
            .Setup(t => t.AddAsync(It.IsAny<Account>()))
            .ReturnsAsync(account);

        // Act
        var response = await service.AddAsync(account);

        // Assert
        response.Should().BeEquivalentTo(account);
        accountRepository.Verify(t => t.AddAsync(It.Is<Account>(entity =>
        entity.Id == account.Id &&
        entity.Name == account.Name &&
        entity.Description == account.Description
        )), Times.Once());
    }

    [Fact(DisplayName = "Should Update Account Successfully")]
    [Trait(nameof(AccountService), nameof(AccountService.UpdateAsync))]
    public async Task ShouldUpdateAccountSuccessfully()
    {
        // Arrange
        var account = new Account { Name = faker.Lorem.Word(), Description = faker.Lorem.Word() };
        var idAccount = Guid.NewGuid();

        accountRepository
            .Setup(t => t.UpdateAsync(It.IsAny<Account>(), It.IsAny<Guid>()))
            .ReturnsAsync(account);

        // Act
        var response = await service.UpdateAsync(account, idAccount);

        // Assert
        response.Should().BeEquivalentTo(account);
        accountRepository.Verify(t => t.UpdateAsync(It.Is<Account>(entity =>
        entity.Id == account.Id &&
        entity.Name == account.Name &&
        entity.Description == account.Description
        ), idAccount), Times.Once());
    }

    [Fact(DisplayName = "Should Delete Account Successfully")]
    [Trait(nameof(AccountService), nameof(AccountService.DeleteAsync))]
    public async Task ShouldDeleteAccountSuccessfully()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await service.DeleteAsync(id);

        // Assert
        accountRepository.Verify(t => t.DeleteAsync(id), Times.Once());
    }
}
