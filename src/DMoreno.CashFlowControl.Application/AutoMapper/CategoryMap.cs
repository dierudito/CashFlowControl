using AutoMapper;
using DMoreno.CashFlowControl.Application.ViewModels.Requests;
using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using DMoreno.CashFlowControl.Domain.Entities;

namespace DMoreno.CashFlowControl.Application.AutoMapper;

public class CategoryMap : Profile
{
    public CategoryMap()
    {
        CreateMap<CategoryRequestViewModel, Category>();
        CreateMap<Category, CategoryResponseViewModel>();
    }
}
