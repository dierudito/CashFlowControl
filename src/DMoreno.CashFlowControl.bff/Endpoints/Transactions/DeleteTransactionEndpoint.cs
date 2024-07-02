using DMoreno.CashFlowControl.Application.Interfaces;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.bff.Extensions;

namespace DMoreno.CashFlowControl.bff.Endpoints.Transactions;

public class DeleteTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapDelete("/{idTransaction:Guid}", HandleAsync)
        .WithName("Transaction: Delete")
        .WithSummary("Deletes a transaction")
        .WithDescription("Deletes a transaction")
        .WithOrder(3)
        .Produces<Response<bool>>();

    private static async Task<IResult> HandleAsync(
        ITransactionAppService appService,
        Guid idTransaction)
    {
        var response = await appService.DeleteAsync(idTransaction);
        return ResponseResult<bool>.CreateResponse(response);
    }
}