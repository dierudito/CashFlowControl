using AutoMapper;
using DMoreno.CashFlowControl.Application.Interfaces;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.Domain.Extensions;
using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Infra.CrossCutting.Shared;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DMoreno.CashFlowControl.Application.AppServices;
public class DailyConsolidatedBalanceAppService(
    ILogger<DailyConsolidatedBalanceAppService> logger,
    IMapper mapper,
    ICashFlowRepository cashFlowRepository) : IDailyConsolidatedBalanceAppService
{
    public async Task<Response<List<DailyConsolidatedBalanceResponseViewModel>>> GetByPeriodAsync(DailyConsolidatedBalanceRequestViewModel request)
    {
		try
		{
			request.StartDate ??= DateTime.Now.GetFirstDay().DateOnly();
			request.EndDate ??= DateTime.Now.GetLastDay().DateOnly();

            var periodSize = 
                (int)Math.Ceiling((request.EndDate.Value.ToDateTime(new()) - 
                request.StartDate.Value.ToDateTime(new())).TotalDays);

            if (periodSize > ApiConfigurations.MaxLengthPeriodDays)
            {
                logger.LogWarning("O período de {PeriodSize} dias é maior que permitido de {MaxLength} dias", periodSize, ApiConfigurations.MaxLengthPeriodDays);
                return new(null, HttpStatusCode.BadRequest, $"O período não pode ser maior que {ApiConfigurations.MaxLengthPeriodDays} dias");
            }
        }
		catch (Exception e)
		{
			logger.LogError(e, "Não foi possível determinar a data do período");
			return new(null, HttpStatusCode.Unauthorized, "Não foi possível determinar a data do período");
		}

        logger.LogInformation("Iniciando processo para geração do saldo diário consolidado para o período de {@Period}", request);

        var dailyConsolidated = await cashFlowRepository.GetByPeriodAsync(request.StartDate.Value, request.EndDate.Value);

        if (dailyConsolidated.Count == 0)
        {
            logger.LogInformation("Transações não encontradas para o período {@Period}", request);
            return new(null, HttpStatusCode.NoContent, "Não foi encontrado transação para o período informado");
        }

        var dailyResponse = mapper.Map<List<DailyConsolidatedBalanceResponseViewModel>>(dailyConsolidated);

        return new(dailyResponse, HttpStatusCode.OK, "Consolidado gerado");
    }
}
