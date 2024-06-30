using AutoMapper;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.Domain.Entities;

namespace DMoreno.CashFlowControl.Application.AutoMapper;
public class TransactionMap : Profile
{
    public TransactionMap()
    {
        CreateMap<AddTransactionRequestViewModel, Transaction>()
            .ForMember(t => t.Amount, m => m.MapFrom(src => src.Amount < 0 ? src.Amount * -1 : src.Amount));
        CreateMap<UpdateTransactionRequestViewModel, Transaction>()
            .ForMember(t => t.Amount, m => m.MapFrom(src => src.Amount < 0 ? src.Amount * -1 : src.Amount));
        CreateMap<Transaction, AddTransactionResponseViewModel>()
            .ForMember(t => t.Date, m => m.MapFrom(src => src.Date.ToString("dd/MM/yyyy HH:mm")))
            .ForMember(t => t.Amount, m => m.MapFrom(src => src.Amount.ToString("C2")));
        CreateMap<Transaction, GetTransactionByIdResponseViewModel>()
            .ForMember(t => t.Date, m => m.MapFrom(src => src.Date.ToString("dd/MM/yyyy HH:mm")))
            .ForMember(t => t.Amount, m => m.MapFrom(src => src.Amount.ToString("C2")));
    }
}
