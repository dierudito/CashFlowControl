using DMoreno.CashFlowControl.Application.Interfaces;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.bff.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DMoreno.CashFlowControl.bff.Endpoints.Consolidated;

public class GetConsolidatedByPeriodEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/", HandleAsync)
        .WithName("Consolidated: Get by period")
        .WithSummary("Get consolidated by period")
        .WithDescription("Get consolidated by period")
        .WithOrder(1)
        .Produces<Response<List<DailyConsolidatedBalanceResponseViewModel>>>();

    private static async Task<IResult> HandleAsync(
        IDailyConsolidatedBalanceAppService appService,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var request = new DailyConsolidatedBalanceRequestViewModel(startDate, endDate);
        var response = await appService.GetByPeriodAsync(request);
        return ResponseResult<List<DailyConsolidatedBalanceResponseViewModel>>.CreateResponse(response);
    }
}
