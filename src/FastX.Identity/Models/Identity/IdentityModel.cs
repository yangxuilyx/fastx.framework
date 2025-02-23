using FastX.Identity.Core.Identity.Users;

namespace FastX.Identity.Models.Identity;

/// <summary>
/// 用户信息
/// </summary>
public class IdentityModel
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 组织Id
    /// </summary>
    public string? OuId { get; set; }

    /// <summary>
    /// 是否超管
    /// </summary>
    public bool IsSpecial { get; set; }
}