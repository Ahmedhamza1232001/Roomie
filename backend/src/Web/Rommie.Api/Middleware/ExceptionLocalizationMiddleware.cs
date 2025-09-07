using Rommie.Domain.Abstractions;

namespace Rommie.Api.Middleware;

public class ExceptionLocalizationMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionLocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
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
            var localizedMessage = key + " " + String.Join(", ", ex.MessageArgs);
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                code = ex.ErrorCode,
                status = ex.StatusCode,
                message = localizedMessage
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
                message = "INTERNAL_ERROR_500"
            });
        }
    }
}

