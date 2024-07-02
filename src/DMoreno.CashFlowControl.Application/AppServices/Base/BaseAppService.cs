using DMoreno.CashFlowControl.Domain.Interfaces.UoW;

namespace DMoreno.CashFlowControl.Application.AppServices.Base;
public abstract class BaseAppService(IUnitOfWork uow)
{
    protected async Task SaveChangesAsync() =>
        await uow.CommitAsync();
}
