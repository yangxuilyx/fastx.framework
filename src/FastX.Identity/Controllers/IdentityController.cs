using FastX.AspNetCore.Controllers;
using FastX.Data.Repository;
using FastX.Identity.Core.Identity.Roles;
using FastX.Identity.Core.Identity.Users;
using FastX.Identity.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastX.Identity.Controllers;

/// <summary>
/// 身份管理
/// </summary>
[Authorize()]
public class IdentityController : XController
{
    private readonly UserManager _userManager;

    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IRepository<RolePermission> _rolePermissionsRepository;

    /// <summary>
    /// 
    /// </summary>
    public IdentityController(UserManager userManager, IRepository<UserRole> userRoleRepository, IRepository<RolePermission> rolePermissionsRepository)
    {
        _userManager = userManager;
        _userRoleRepository = userRoleRepository;
        _rolePermissionsRepository = rolePermissionsRepository;
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IdentityModel> GetUserInfo()
    {
        var user = await _userManager.GetUserByIdAsync(CurrentUser.UserId);
        if (user == null)
            throw new UserFriendlyException("用户不存在");

        return new IdentityModel
        {
            UserId = user.UserId,
            UserName = user.UserName,
            Name = user.Name,
            OuId = user.OuId?.ToString(),
            IsSpecial = user.IsSpecial ?? false,
            PositionType = user.PositionType
        };
    }

    [HttpGet]
    public async Task<List<string>> Permissions()
    {
        var user = await _userManager.GetUserByIdAsync(CurrentUser.UserId);
        if (user == null)
            throw new UserFriendlyException("用户不存在");

        var userRoles = await _userRoleRepository.GetListAsync(p => p.UserId == user.UserId);

        return (await Task.WhenAll(userRoles.Select(async userRole =>
        {
            var rolePermissions = await _rolePermissionsRepository.GetListAsync(p => p.RoleId == userRole.RoleId);

            return rolePermissions.Select(t => t.Name);
        }))).SelectMany(p => p).ToList();
    }
}