using FastX.AspNetCore;
using FastX.Identity.Application.Identity.Ous.Dtos;

namespace FastX.Identity.Application.Identity.Ous;

/// <summary>
/// 组织管理
/// </summary>
public interface IOuAppService : IApplicationService
{
    /// <summary>
    /// 获取所有组织
    /// </summary>
    /// <returns></returns>
    Task<List<OuDto>> GetAllAsync();
}