using FastX;
using FastX.Modularity;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection;

public static class WebApplicationBuilderExtensions
{
    public static async Task<IXApplication> AddApplicationAsync<TStartupModule>(
       this WebApplicationBuilder builder
    ) where TStartupModule : IXModule
    {
       return await builder.Services.AddApplicationAsync<TStartupModule>();
    }
}