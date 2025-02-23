using AutoMapper;
using FastX.AspNetCore;
using FastX.DependencyInjection;
using FastX.MultiTenancy;
using FastX.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace FastX.Application.Services;

public abstract class ApplicationService : IApplicationService, ITransientDependency
{
    public static string[] CommonPostfixes { get; set; } = { "AppService", "ApplicationService", "Service" };

    public IServiceProvider ServiceProvider { get; set; } = default!;

    protected ICurrentTenant CurrentTenant => ServiceProvider.GetRequiredService<ICurrentTenant>();

    protected ICurrentUser CurrentUser => ServiceProvider.GetRequiredService<ICurrentUser>();

    protected IMapper ObjectMapper => ServiceProvider.GetRequiredService<IMapper>();
}