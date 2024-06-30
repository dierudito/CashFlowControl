using DMoreno.CashFlowControl.Application.Interfaces;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.bff.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DMoreno.CashFlowControl.bff.Endpoints.Categories;

public class UpdateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPut("/{idCategory:Guid}", HandleAsync)
        .WithName("Category: Update")
        .WithSummary("Updates a category")
        .WithDescription("Updates a category")
        .WithOrder(2)
        .Produces<Response<bool>>();

    private static async Task<IResult> HandleAsync(
        ICategoryAppService appService,
        [FromRoute] Guid idCategory,
        [FromBody] CategoryRequestViewModel request)
    {
        var response = await appService.UpdateAsync(request, idCategory);
        return ResponseResult<bool>.CreateResponse(response);
    }
}