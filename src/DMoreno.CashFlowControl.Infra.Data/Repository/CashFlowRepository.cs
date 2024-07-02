using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Infra.Data.Context.Entity;
using DMoreno.CashFlowControl.Infra.Data.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace DMoreno.CashFlowControl.Infra.Data.Repository;
public class CashFlowRepository(CashFlowControlDbContext db) :
    BaseRepository<CashFlow>(db), ICashFlowRepository
{
    public async Task<IList<CashFlow>> GetByPeriodAsync(DateOnly startDate, DateOnly endDate) =>
        await _dbSet
        .Where(entity => entity.ReleaseDate >= startDate && entity.ReleaseDate <= endDate)
        .OrderByDescending(entity => entity.ReleaseDate)
        .ToListAsync();

    public async Task<CashFlow?> GetByDateAsync(DateOnly date) =>
        await _dbSet.FirstOrDefaultAsync(entity => entity.ReleaseDate == date);

    public async Task<CashFlow?> GetLatestAsync() =>
        await _dbSet.OrderByDescending(entity => entity.ReleaseDate).LastOrDefaultAsync();
}