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
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}