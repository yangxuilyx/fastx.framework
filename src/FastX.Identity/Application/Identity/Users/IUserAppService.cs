using FastX.AspNetCore;
using FastX.Identity.Application.Identity.Users.Dtos;

namespace FastX.Identity.Application.Identity.Users;

/// <summary>
/// 用户管理
/// </summary>
public interface IUserAppService : IApplicationService
{
    /// <summary>
    /// 获取所有组织
    /// </summary>
    /// <returns></returns>
    Task<List<UserDto>> GetAllAsync();
}