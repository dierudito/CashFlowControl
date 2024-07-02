using DMoreno.CashFlowControl.Application.Interfaces;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.bff.Extensions;

namespace DMoreno.CashFlowControl.bff.Endpoints.Accounts;

public class GetAllAccountEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/", HandleAsync)
        .WithName("Account: Get all")
        .WithSummary("Retrieves all accounts")
        .WithDescription("Retrieves all accounts")
        .WithOrder(5)
        .Produces<Response<List<AccountResponseViewModel>>>();

    private static async Task<IResult> HandleAsync(
        IAccountAppService appService)
    {
        var response = await appService.GetAllAsync();
        return ResponseResult<List<AccountResponseViewModel>>.CreateResponse(response);
    }
}
