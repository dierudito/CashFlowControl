using DMoreno.CashFlowControl.Application.Interfaces;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.bff.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DMoreno.CashFlowControl.bff.Endpoints.Accounts;

public class UpdateAccountEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPut("/{idAccount:Guid}", HandleAsync)
        .WithName("Account: Update")
        .WithSummary("Updates a account")
        .WithDescription("Updates a account")
        .WithOrder(2)
        .Produces<Response<bool>>();

    private static async Task<IResult> HandleAsync(
        IAccountAppService appService,
        [FromRoute] Guid idAccount,
        [FromBody] AccountRequestViewModel request)
    {
        var response = await appService.UpdateAsync(request, idAccount);
        return ResponseResult<bool>.CreateResponse(response);
    }
}