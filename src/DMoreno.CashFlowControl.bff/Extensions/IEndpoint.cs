namespace DMoreno.CashFlowControl.bff.Extensions;

public interface IEndpoint
{
    static abstract void Map(IEndpointRouteBuilder app);
}