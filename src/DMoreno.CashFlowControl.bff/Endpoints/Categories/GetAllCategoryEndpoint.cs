using DMoreno.CashFlowControl.Application.Interfaces;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.bff.Extensions;

namespace DMoreno.CashFlowControl.bff.Endpoints.Categories;

public class GetAllCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/", HandleAsync)
        .WithName("Category: Get all")
        .WithSummary("Retrieves all categories")
        .WithDescription("Retrieves all categories")
        .WithOrder(5)
        .Produces<Response<List<CategoryResponseViewModel>>>();

    private static async Task<IResult> HandleAsync(
        ICategoryAppService appService)
    {
        var response = await appService.GetAllAsync();
        return ResponseResult<List<CategoryResponseViewModel>>.CreateResponse(response);
    }
}
