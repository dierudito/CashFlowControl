using DMoreno.CashFlowControl.Domain.Entities;

namespace DMoreno.CashFlowControl.Domain.Interfaces.Services;

public interface ITransactionService
{
    Task<Transaction> AddAsync(Transaction transaction);

    Task<Transaction> UpdateAsync(Transaction transaction);

    Task DeleteAsync(Guid id);
}
