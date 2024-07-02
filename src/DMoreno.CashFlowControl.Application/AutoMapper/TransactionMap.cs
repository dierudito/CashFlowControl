using AutoMapper;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.Domain.Entities;
using DMoreno.CashFlowControl.Domain.Enums;

namespace DMoreno.CashFlowControl.Application.AutoMapper;
public class TransactionMap : Profile
{
    public TransactionMap()
    {
        CreateMap<AddTransactionRequestViewModel, Transaction>()
            .ForMember(t => t.Amount, m => m.MapFrom(src => src.Amount < 0 ? src.Amount * -1 : src.Amount))
            .ForMember(t => t.Type, m => m.MapFrom(src => src.Amount < 0 ? ETransactionType.debit : ETransactionType.credit));

        CreateMap<UpdateTransactionRequestViewModel, Transaction>()
            .ForMember(t => t.Amount, m => m.MapFrom(src => src.Amount < 0 ? src.Amount * -1 : src.Amount))
            .ForMember(t => t.Type, m => m.MapFrom(src => src.Amount < 0 ? ETransactionType.debit : ETransactionType.credit));

        CreateMap<Transaction, AddTransactionResponseViewModel>()
            .ForMember(t => t.Amount, m => m.MapFrom(src => (src.Type == ETransactionType.debit ? src.Amount * -1 : src.Amount).ToString("C2")));
        CreateMap<Transaction, GetTransactionByIdResponseViewModel>()
            .ForMember(t => t.Date, m => m.MapFrom(src => src.CashFlow.ReleaseDate.ToString("dd/MM/yyyy")))
            .ForMember(t => t.Amount, m => m.MapFrom(src => (src.Type == ETransactionType.debit ? src.Amount * -1 : src.Amount).ToString("C2")));
    }
}
