using DMoreno.CashFlowControl.Application.Interfaces;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.bff.Extensions;

namespace DMoreno.CashFlowControl.bff.Endpoints.Accounts;

public class GetByIdAccountEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/{idAccount:Guid}", HandleAsync)
        .WithName("Account: Get Unique")
        .WithSummary("Gets a account")
        .WithDescription("Gets a account")
        .WithOrder(4)
        .Produces<Response<AccountResponseViewModel>>();

    private static async Task<IResult> HandleAsync(
        IAccountAppService appService,
        Guid idAccount)
    {
        var response = await appService.GetByIdAsync(idAccount);
        return ResponseResult<AccountResponseViewModel>.CreateResponse(response);
    }
}
