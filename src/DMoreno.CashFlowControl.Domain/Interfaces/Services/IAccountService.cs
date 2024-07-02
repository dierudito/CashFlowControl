using DMoreno.CashFlowControl.Domain.Entities;

namespace DMoreno.CashFlowControl.Domain.Interfaces.Services;

public interface IAccountService
{
    Task<Account> AddAsync(Account account);

    Task<Account?> UpdateAsync(Account account, Guid idAccount);

    Task DeleteAsync(Guid id);
}
