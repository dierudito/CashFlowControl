using DMoreno.CashFlowControl.bff.Endpoints.Accounts;
using DMoreno.CashFlowControl.bff.Endpoints.Categories;
using DMoreno.CashFlowControl.bff.Endpoints.Transactions;
using DMoreno.CashFlowControl.bff.Extensions;
using DMoreno.CashFlowControl.Infra.CrossCutting.Shared;

namespace DMoreno.CashFlowControl.bff.Endpoints;


public static class Endpoint
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("");

        endpoints.MapGroup("/")
            .WithTags("Health Check")
            .MapGet("/", () => new { message = "OK" });

        endpoints.MapGroup($"v1/{ApiConfigurations.RouteTransaction}")
            .WithTags("Transactions")
            .MapEndpoint<CreateTransactionEndpoint>()
            .MapEndpoint<UpdateTransactionEndpoint>()
            .MapEndpoint<DeleteTransactionEndpoint>()
            .MapEndpoint<GetTransactionByIdEndpoint>();

        endpoints.MapGroup($"v1/{ApiConfigurations.RouteCategory}")
            .WithTags("Categories")
            .MapEndpoint<CreateCategoryEndpoint>()
            .MapEndpoint<UpdateCategoryEndpoint>()
            .MapEndpoint<GetAllCategoryEndpoint>()
            .MapEndpoint<GetByIdCategoryEndpoint>()
            .MapEndpoint<DeleteCategoryEndpoint>();

        endpoints.MapGroup($"v1/{ApiConfigurations.RouteAccount}")
            .WithTags("Accounts")
            .MapEndpoint<CreateAccountEndpoint>()
            .MapEndpoint<UpdateAccountEndpoint>()
            .MapEndpoint<GetAllAccountEndpoint>()
            .MapEndpoint<GetByIdAccountEndpoint>()
            .MapEndpoint<DeleteAccountEndpoint>();
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}