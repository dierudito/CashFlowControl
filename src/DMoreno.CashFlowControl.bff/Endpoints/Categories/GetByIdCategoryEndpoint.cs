using DMoreno.CashFlowControl.Application.Interfaces;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.bff.Extensions;

namespace DMoreno.CashFlowControl.bff.Endpoints.Categories;

public class GetByIdCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/{idCategory:Guid}", HandleAsync)
        .WithName("Category: Get Unique")
        .WithSummary("Gets a category")
        .WithDescription("Gets a category")
        .WithOrder(4)
        .Produces<Response<CategoryResponseViewModel>>();

    private static async Task<IResult> HandleAsync(
        ICategoryAppService appService,
        Guid idCategory)
    {
        var response = await appService.GetByIdAsync(idCategory);
        return ResponseResult<CategoryResponseViewModel>.CreateResponse(response);
    }
}
