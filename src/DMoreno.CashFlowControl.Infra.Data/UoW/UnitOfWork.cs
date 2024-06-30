using DMoreno.CashFlowControl.Domain.Interfaces.UoW;
using DMoreno.CashFlowControl.Infra.Data.Context;

namespace DMoreno.CashFlowControl.Infra.Data.UoW;
public class UnitOfWork(CashFlowControlDbContext db) : IUnitOfWork
{
    public async Task<int> CommitAsync() =>
        await db.SaveChangesAsync();
}
