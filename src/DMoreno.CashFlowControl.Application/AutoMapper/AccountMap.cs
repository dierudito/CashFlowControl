using AutoMapper;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.Domain.Entities;

namespace DMoreno.CashFlowControl.Application.AutoMapper;

public class AccountMap : Profile
{
    public AccountMap()
    {
        CreateMap<AccountRequestViewModel, Account>();
        CreateMap<Account, AccountResponseViewModel>();
    }
}
