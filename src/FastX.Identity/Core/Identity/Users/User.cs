using FastX.Data.Entities;
using FastX.Data.SqlSugar.DataAnnotations;

namespace FastX.Identity.Core.Identity.Users;

/// <summary>
/// 用户
/// </summary>
[XSugarTable("Identity")]
public class User : AuditEntity
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public Ulid UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 身份证号
    /// </summary>
    public string? IdNo { get; set; }

    /// <summary>
    /// 组织Id
    /// </summary>
    public Ulid? OuId { get; set; }

    /// <summary>
    /// 是否超管
    /// </summary>
    public bool? IsSpecial { get; set; }
}
