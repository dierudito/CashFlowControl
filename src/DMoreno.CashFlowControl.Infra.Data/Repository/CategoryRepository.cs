using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Infra.Data.Context.Entity;
using DMoreno.CashFlowControl.Infra.Data.Repository.Base;

namespace DMoreno.CashFlowControl.Infra.Data.Repository;
public class CategoryRepository(CashFlowControlDbContext db) : 
    BaseRepository<Category>(db), ICategoryRepository
{
}
