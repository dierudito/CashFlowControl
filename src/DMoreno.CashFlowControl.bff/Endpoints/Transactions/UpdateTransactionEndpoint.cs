using DMoreno.CashFlowControl.Application.Interfaces;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.bff.Extensions;

namespace DMoreno.CashFlowControl.bff.Endpoints.Transactions;

public class UpdateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPut("/{idTransaction:Guid}", HandleAsync)
        .WithName("Transaction: Update")
        .WithSummary("Updates a transaction")
        .WithDescription("Updates a transaction")
        .WithOrder(2)
        .Produces<Response<bool>>();

    private static async Task<IResult> HandleAsync(
        ITransactionAppService appService,
        UpdateTransactionRequestViewModel request,
        Guid idTransaction)
    {
        var response = await appService.UpdateAsync(request, idTransaction);
        return ResponseResult<bool>.CreateResponse(response);
    }
}
