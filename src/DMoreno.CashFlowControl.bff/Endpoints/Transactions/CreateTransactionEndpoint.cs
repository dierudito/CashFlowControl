using DMoreno.CashFlowControl.Application.Interfaces;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.bff.Extensions;
using DMoreno.CashFlowControl.Infra.CrossCutting.Shared;

namespace DMoreno.CashFlowControl.bff.Endpoints.Transactions;

public class CreateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPost("/", HandleAsync)
        .WithName("Transaction: Create")
        .WithSummary("Creates a new transaction")
        .WithDescription("Creates a new transaction")
        .WithOrder(1)
        .Produces<Response<AddTransactionResponseViewModel>>();

    private static async Task<IResult> HandleAsync(ITransactionAppService appService, AddTransactionRequestViewModel request)
    {
        var response = await appService.AddAsync(request);
        return ResponseResult<AddTransactionResponseViewModel>.CreateResponse(response);
    }
}
