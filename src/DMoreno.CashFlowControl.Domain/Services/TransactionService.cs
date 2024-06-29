using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Domain.Interfaces.Services;

namespace DMoreno.CashFlowControl.Domain.Services;

public class TransactionService(ITransactionRepository transactionRepository) : ITransactionService
{
    public async Task<Transaction> AddAsync(Transaction transaction) =>
        await transactionRepository.AddAsync(transaction);

    public async Task<Transaction> UpdateAsync(Transaction transaction) =>
        await transactionRepository.UpdateAsync(transaction);

    public async Task DeleteAsync(Guid id) =>
        await transactionRepository.DeleteAsync(id);
}
