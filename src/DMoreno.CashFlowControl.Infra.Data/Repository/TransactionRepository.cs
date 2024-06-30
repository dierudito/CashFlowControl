using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Infra.Data.Context;
using DMoreno.CashFlowControl.Infra.Data.Repository.Base;

namespace DMoreno.CashFlowControl.Infra.Data.Repository;
public class TransactionRepository(CashFlowControlDbContext db) :
    BaseRepository<Transaction>(db), ITransactionRepository
{
}
