using DMoreno.CashFlowControl.Application.Interfaces;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.bff.Extensions;

namespace DMoreno.CashFlowControl.bff.Endpoints.Accounts;

public class CreateAccountEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPost("/", HandleAsync)
        .WithName("Account: Create")
        .WithSummary("Creates a new account")
        .WithDescription("Creates a new account")
        .WithOrder(1)
        .Produces<Response<AccountResponseViewModel>>();

    private static async Task<IResult> HandleAsync(IAccountAppService appService, AccountRequestViewModel request)
    {
        var response = await appService.AddAsync(request);
        return ResponseResult<AccountResponseViewModel>.CreateResponse(response);
    }
}