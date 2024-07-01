using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Infra.Data.Context.Entity;
using DMoreno.CashFlowControl.Infra.Data.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace DMoreno.CashFlowControl.Infra.Data.Repository;
public class TransactionRepository(CashFlowControlDbContext db) :
    BaseRepository<Transaction>(db), ITransactionRepository
{
    public override async Task<Transaction?> GetByIdAsync(Guid id) =>
        await _dbSet.AsNoTracking().Include(entity => entity.CashFlow)
        .FirstOrDefaultAsync(entity => entity.Id == id);
}
