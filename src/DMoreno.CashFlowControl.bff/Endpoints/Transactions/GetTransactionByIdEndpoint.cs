using DMoreno.CashFlowControl.Application.Interfaces;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.bff.Extensions;

namespace DMoreno.CashFlowControl.bff.Endpoints.Transactions;

public class GetTransactionByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/{id:Guid}", HandleAsync)
        .WithName("Transaction: Get")
        .WithSummary("Get the transaction")
        .WithDescription("Get the transaction")
        .WithOrder(4)
        .Produces<Response<GetTransactionByIdResponseViewModel>>();

    private static async Task<IResult> HandleAsync(ITransactionAppService appService, Guid id)
    {
        var response = await appService.GetByIdAsync(id);
        return response.IsSuccess
            ? TypedResults.Ok(response)
            : TypedResults.StatusCode((int)response.Code);
    }
}
