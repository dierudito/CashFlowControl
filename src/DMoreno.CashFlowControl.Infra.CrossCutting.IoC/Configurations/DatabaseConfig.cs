using DMoreno.CashFlowControl.Infra.CrossCutting.Shared;
using DMoreno.CashFlowControl.Infra.Data.Context.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DMoreno.CashFlowControl.Infra.CrossCutting.IoC.Configurations;
public static class DatabaseConfig
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services) =>
        services
        .AddEntityFrameworkConfiguration();


    private static IServiceCollection AddEntityFrameworkConfiguration(this IServiceCollection services) =>
        services
        .AddDbContext<CashFlowControlDbContext>(options =>
        {
            options.UseSqlServer(ApiConfigurations.ConncetionString);
#if (DEBUG)
            options.EnableSensitiveDataLogging();
#endif
        });
}
