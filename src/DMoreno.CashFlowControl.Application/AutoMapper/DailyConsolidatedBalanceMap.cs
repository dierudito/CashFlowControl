using AutoMapper;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.Domain.Entities;

namespace DMoreno.CashFlowControl.Application.AutoMapper;

public class DailyConsolidatedBalanceMap : Profile
{
    public DailyConsolidatedBalanceMap()
    {
        CreateMap<CashFlow, DailyConsolidatedBalanceResponseViewModel>()
            .ForMember(d => d.Date, m => m.MapFrom(src => src.ReleaseDate.ToString("dd/MM/yyyy")))
            .ForMember(d => d.TotalCredits, m => m.MapFrom(src => src.TotalCredits.ToString("C2")))
            .ForMember(d => d.TotalDebits, m => m.MapFrom(src => src.TotalDebits.ToString("C2")))
            .ForMember(d => d.OpeningBalance, m => m.MapFrom(src => src.OpeningBalance.ToString("C2")))
            .ForMember(d => d.ClosingBalance, m => m.MapFrom(src => src.ClosingBalance.ToString("C2")));
    }
}