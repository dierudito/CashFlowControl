using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Domain.Interfaces.Services;

namespace DMoreno.CashFlowControl.Domain.Services;

public class AccountService(IAccountRepository accountRepository) : IAccountService
{
    public async Task<Account> AddAsync(Account account) =>
        await accountRepository.AddAsync(account);

    public async Task<Account?> UpdateAsync(Account account, Guid idAccount) =>
        await accountRepository.UpdateAsync(account, idAccount);

    public async Task DeleteAsync(Guid id) =>
        await accountRepository.DeleteAsync(id);
}