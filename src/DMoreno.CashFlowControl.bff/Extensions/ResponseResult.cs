using DMoreno.CashFlowControl.Application.ViewModels.Responses;
using System.Net;

namespace DMoreno.CashFlowControl.bff.Extensions;

public static class ResponseResult<TData>
{
    public static IResult CreateResponse(Response<TData> response, string? redirectRoute = null) => response.Code switch
    {
        HttpStatusCode.OK => TypedResults.Ok(response),
        HttpStatusCode.Created => TypedResults.Created(redirectRoute, response),
        _ => Results.Json(response, statusCode: (int)response.Code)
    };
}
