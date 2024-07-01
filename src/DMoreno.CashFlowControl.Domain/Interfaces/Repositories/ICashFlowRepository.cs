using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories.Base;

namespace DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
public interface ICashFlowRepository : IBaseRepository<CashFlow>
{
    Task<IList<CashFlow>> GetByPeriodAsync(DateOnly startDate, DateOnly endDate);

    Task<CashFlow?> GetByDateAsync(DateOnly date);

    Task<CashFlow?> GetLatestAsync();
}