using DMoreno.CashFlowControl.Application.Interfaces;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.bff.Extensions;

namespace DMoreno.CashFlowControl.bff.Endpoints.Categories;

public class DeleteCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapDelete("/{idCategory:Guid}", HandleAsync)
        .WithName("Category: Delete")
        .WithSummary("Delete a category")
        .WithDescription("Delete a category")
        .WithOrder(3)
        .Produces<Response<bool>>();

    private static async Task<IResult> HandleAsync(ICategoryAppService appService, Guid idCategory)
    {
        var response = await appService.DeleteAsync(idCategory);
        return ResponseResult<bool>.CreateResponse(response);
    }
}
