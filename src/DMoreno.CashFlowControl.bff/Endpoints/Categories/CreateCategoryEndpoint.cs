using DMoreno.CashFlowControl.Application.Interfaces;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.bff.Extensions;

namespace DMoreno.CashFlowControl.bff.Endpoints.Categories;

public class CreateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPost("/", HandleAsync)
        .WithName("Category: Create")
        .WithSummary("Creates a new category")
        .WithDescription("Creates a new category")
        .WithOrder(1)
        .Produces<Response<CategoryResponseViewModel>>();

    private static async Task<IResult> HandleAsync(ICategoryAppService appService, CategoryRequestViewModel request)
    {
        var response = await appService.AddAsync(request);
        return ResponseResult<CategoryResponseViewModel>.CreateResponse(response);
    }
}