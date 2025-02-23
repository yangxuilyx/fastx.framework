using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FastX.AspNetCore;

public static class UseResultWrapperExtensions
{
    public static IApplicationBuilder UseResultWrapper(this IApplicationBuilder app)
    {
        var serializerOptions = app.ApplicationServices.GetRequiredService<IOptions<JsonOptions>>().Value.JsonSerializerOptions;
        return app.Use(async (context, next) =>
        {
            var originalResponseBody = context.Response.Body;
            try
            {
                //因为Response.Body没办法进行直接读取，所以需要特殊操作一下
                using var swapStream = new MemoryStream();
                context.Response.Body = swapStream;
                await next();
                //判断是否出现了异常状态码，需要特殊处理
                if (context.Response.StatusCode == StatusCodes.Status500InternalServerError)
                {
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    await swapStream.CopyToAsync(originalResponseBody);
                    return;
                }
                if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
                {
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    await swapStream.CopyToAsync(originalResponseBody);
                    return;
                }

                var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
                if (endpoint != null)
                {
                    //只针对application/json结果进行处理
                    //if ((!context.Response.ContentType.IsNullOrEmpty() && context.Response.ContentType.ToLower().Contains("application/json")))
                    {
                        //判断终结点是否包含NoWrapperAttribute
                        DontWrapResultAttribute? noWrapper = endpoint.Metadata.GetMetadata<DontWrapResultAttribute>();
                        if (noWrapper != null)
                        {
                            context.Response.Body.Seek(0, SeekOrigin.Begin);
                            await swapStream.CopyToAsync(originalResponseBody);
                            return;
                        }

                        if (context.Response.ContentType.IsNullOrEmpty())
                            context.Response.ContentType = "application/json";

                        //获取Action的返回类型
                        var controllerActionDescriptor = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();
                        if (controllerActionDescriptor != null)
                        {
                            //泛型的特殊处理
                            var returnType = controllerActionDescriptor.MethodInfo.ReturnType;
                            if (returnType.IsGenericType && (returnType.GetGenericTypeDefinition() == typeof(Task<>) || returnType.GetGenericTypeDefinition() == typeof(ValueTask<>)))
                            {
                                returnType = returnType.GetGenericArguments()[0];
                            }
                            //如果终结点已经是ResponseResult<T>则不进行包装处理
                            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ResponseResult<>))
                            {
                                context.Response.Body.Seek(0, SeekOrigin.Begin);
                                await swapStream.CopyToAsync(originalResponseBody);
                                return;
                            }

                            if (returnType == typeof(Task))
                            {
                                var bytes = JsonSerializer.SerializeToUtf8Bytes(ResponseResult<object>.SuccessResult(null), serializerOptions);
                                await new MemoryStream(bytes).CopyToAsync(originalResponseBody);
                                return;
                            }
                            else
                            {

                                context.Response.Body.Seek(0, SeekOrigin.Begin);
                                //反序列化得到原始结果
                                var result = await JsonSerializer.DeserializeAsync(context.Response.Body, returnType, serializerOptions);
                                //对原始结果进行包装
                                var bytes = JsonSerializer.SerializeToUtf8Bytes(ResponseResult<object>.SuccessResult(result), serializerOptions);
                                await new MemoryStream(bytes).CopyToAsync(originalResponseBody);
                                return;
                            }
                        }
                    }
                }
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await swapStream.CopyToAsync(originalResponseBody);
            }
            finally
            {
                //将原始的Body归还回来
                context.Response.Body = originalResponseBody;
            }
        });
    }
}