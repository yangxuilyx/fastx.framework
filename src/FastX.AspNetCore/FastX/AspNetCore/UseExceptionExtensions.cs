using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FastX.AspNetCore;

public static class UseExceptionExtensions
{
    public static IApplicationBuilder UseException(this IApplicationBuilder app)
    {
        var serializerOptions = app.ApplicationServices.GetRequiredService<IOptions<JsonOptions>>().Value.JsonSerializerOptions;
        serializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        return app.UseExceptionHandler(configure =>
        {
            configure.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var ex = exceptionHandlerPathFeature?.Error;
                if (ex != null)
                {
                    var rspResult = ResponseResult<object>.ErrorResult(ex.Message);
                    if (ex is UserFriendlyException userFriendlyException)
                        rspResult.Details = userFriendlyException.Details;

                    var logger = context.RequestServices.GetService<ILogger<IExceptionHandlerPathFeature>>();
                    logger?.LogError(ex, message: ex.Message);

                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json;charset=utf-8";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(rspResult, serializerOptions));
                }
            });
        });
    }
}