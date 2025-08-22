using Rommie.Api.Resources;
using Rommie.Domain.Abstractions;
using Microsoft.Extensions.Localization;

namespace Rommie.Api.Middleware;

public class ExceptionLocalizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public ExceptionLocalizationMiddleware(RequestDelegate next, IStringLocalizer<SharedResource> localizer)
    {
        _next = next;
        _localizer = localizer;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (LocalizedHttpException ex)
        {
            var key = $"{ex.ErrorCode}_{ex.StatusCode}";
            var localizedMessage = _localizer[key, ex.MessageArgs];

            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                code = ex.ErrorCode,
                status = ex.StatusCode,
                message = localizedMessage.Value
            });
        }
        catch (Exception)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                code = "INTERNAL_ERROR",
                status = 500,
                message = _localizer["INTERNAL_ERROR_500"]
            });
        }
    }
}

