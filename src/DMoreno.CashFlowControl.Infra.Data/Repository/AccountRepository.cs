using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Infra.Data.Context;
using DMoreno.CashFlowControl.Infra.Data.Repository.Base;

namespace DMoreno.CashFlowControl.Infra.Data.Repository;

public class AccountRepository(CashFlowControlDbContext db) :
    BaseRepository<Account>(db), IAccountRepository
{
}
