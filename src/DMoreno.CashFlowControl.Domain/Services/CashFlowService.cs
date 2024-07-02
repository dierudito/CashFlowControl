using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Extensions;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DMoreno.CashFlowControl.Domain.Services;
public class CashFlowService(
    ICashFlowRepository cashflowRepository,
    ILogger<CashFlowService> logger) : ICashFlowService
{
    public async Task<CashFlow> AddAsync(CashFlow cashflow) =>
        await cashflowRepository.AddAsync(cashflow);

    public async Task<CashFlow> AddWithPreviousBalanceAsync()
    {
        logger.LogInformation("Iniciando processo para adição de um novo fluxo de caixa com base nos registros anteriores");

        var cashFlow = new CashFlow { ReleaseDate = DateTime.Now.DateOnly() };
        var latestCashFlow = await cashflowRepository.GetLatestAsync();

        if (latestCashFlow?.ReleaseDate == cashFlow.ReleaseDate) return latestCashFlow;

        cashFlow.OpeningBalance = latestCashFlow?.ClosingBalance ?? 0;

        cashFlow.SetClosingBalance();
        cashFlow = await AddAsync(cashFlow);
        await cashflowRepository.SaveChangesAsync();
        return cashFlow;
    }

    public async Task<CashFlow?> UpdateAsync(CashFlow cashflow, Guid idCashFlow) =>
        await cashflowRepository.UpdateAsync(cashflow, idCashFlow);

    public async Task DeleteAsync(Guid id) =>
        await cashflowRepository.DeleteAsync(id);

    public async Task<CashFlow> GetOrCreateByDateAsync(DateOnly date)
    {
        logger.LogInformation("Iniciando processo para obtenção ou criação do fluxo de caixa para a data {Date}", date.ToString("dd/MM/yyyy"));

        var cashFlow = await cashflowRepository.GetByDateAsync(date);

        return cashFlow ?? await AddWithPreviousBalanceAsync();
    }
}