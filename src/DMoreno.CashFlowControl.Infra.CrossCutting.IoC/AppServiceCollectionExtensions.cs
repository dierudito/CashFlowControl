using DMoreno.CashFlowControl.Domain.Interfaces.Repositories;
using DMoreno.CashFlowControl.Domain.Interfaces.Services;
using DMoreno.CashFlowControl.Domain.Interfaces.UoW;
using DMoreno.CashFlowControl.Domain.Services;
using DMoreno.CashFlowControl.Infra.CrossCutting.IoC.Configurations;
using DMoreno.CashFlowControl.Infra.Data.Repository;
using DMoreno.CashFlowControl.Infra.Data.UoW;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace DMoreno.CashFlowControl.Infra.CrossCutting.IoC;
public static class AppServiceCollectionExtensions
{
    public static IServiceCollection ResolveDependencies(this IServiceCollection services) =>
        services
        .AddApplicationBase()
        .AddAutoMapper()
        .AddLoggerFactory()
        .AddRepositories()
        .AddServices();

    private static IServiceCollection AddApplicationBase(this IServiceCollection services) =>
        services
        .AddScoped<IUnitOfWork, UnitOfWork>();

    private static IServiceCollection AddLoggerFactory(this IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.ClearProviders();

            builder.AddConsole();

            builder.AddFilter<ConsoleLoggerProvider>("", LogLevel.Information);
        });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services
        .AddTransient<ITransactionRepository, TransactionRepository>();

    private static IServiceCollection AddServices(this IServiceCollection services) =>
        services
        .AddTransient<ITransactionService, TransactionService>();
}
