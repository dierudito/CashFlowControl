using DMoreno.CashFlowControl.Domain.Entities;

namespace DMoreno.CashFlowControl.Domain.Interfaces.Services;
public interface ICashFlowService
{
    Task<CashFlow> AddAsync(CashFlow cashflow);

    Task<CashFlow> AddWithPreviousBalanceAsync();

    Task<CashFlow?> UpdateAsync(CashFlow cashflow, Guid idCashFlow);

    Task DeleteAsync(Guid id);

    Task<CashFlow> GetOrCreateByDateAsync(DateOnly date);
}