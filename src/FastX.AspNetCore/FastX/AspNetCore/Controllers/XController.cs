using FastX.MultiTenancy;
using FastX.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace FastX.AspNetCore.Controllers;

/// <summary>
/// XController
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public abstract class XController : ControllerBase
{
    /// <summary>
    /// ServiceProvider
    /// </summary>
    public IServiceProvider ServiceProvider { get; set; } = default!;

    /// <summary>
    /// CurrentTenant
    /// </summary>
    protected ICurrentTenant CurrentTenant => ServiceProvider.GetRequiredService<ICurrentTenant>();

    /// <summary>
    /// CurrentUser
    /// </summary>
    protected ICurrentUser CurrentUser => ServiceProvider.GetRequiredService<ICurrentUser>();
}