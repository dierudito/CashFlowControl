using AutoMapper;
using DMoreno.CashFlowControl.Application.AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace DMoreno.CashFlowControl.Infra.CrossCutting.IoC.Configurations;
public static class AutoMapperConfig
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        var mapConfig = new MapperConfiguration(mc =>
        {
            mc.AllowNullDestinationValues = true;
            mc.AllowNullCollections = true;

            mc.AddProfile(new TransactionMap());
            mc.AddProfile(new AccountMap());
            mc.AddProfile(new CategoryMap());
            mc.AddProfile(new DailyConsolidatedBalanceMap());
        });

        var mapper = mapConfig.CreateMapper();
        return services.AddSingleton(mapper);
    }
}
