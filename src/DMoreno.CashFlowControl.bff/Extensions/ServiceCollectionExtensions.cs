using DMoreno.CashFlowControl.Application.AppServices;
using DMoreno.CashFlowControl.Application.Interfaces;
using DMoreno.CashFlowControl.Infra.CrossCutting.IoC;
using DMoreno.CashFlowControl.Infra.CrossCutting.IoC.Configurations;
using DMoreno.CashFlowControl.Infra.CrossCutting.Shared;

namespace DMoreno.CashFlowControl.bff.Extensions;

public static class ServiceCollectionExtension
{
    internal static IServiceCollection RegisterServices(this IServiceCollection services) =>
        services
        .AddDatabaseConfiguration()
        .AddCrossOrigin()
        .AddDocumentation()
        .ResolveDependencies()
        .AddAppServices();

    private static IServiceCollection AddAppServices(this IServiceCollection services) =>
        services
        .AddTransient<ITransactionAppService, TransactionAppService>();

    public static IServiceCollection AddDocumentation(this IServiceCollection services) =>
        services
        .AddEndpointsApiExplorer()
        .AddSwaggerGen(x =>
        {
            x.CustomSchemaIds(n => n.FullName);
        });

    public static IServiceCollection AddCrossOrigin(this IServiceCollection services) =>
        services.AddCors(
            options => options.AddPolicy(
                ApiConfigurations.CorsPolicyName,
            policy => policy
                .WithOrigins([
                    ApiConfigurations.BackendUrl,
                    ApiConfigurations.FrontendUrl
                    ])
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                ));
}
