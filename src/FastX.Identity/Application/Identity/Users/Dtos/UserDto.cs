using FastX.Identity.Core.Identity.Users;

namespace FastX.Identity.Application.Identity.Users.Dtos;

public class UserDto
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
    /// 手机号码
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// 身份证号
    /// </summary>
    public string IdNo { get; set; }

    /// <summary>
    /// 组织Id
    /// </summary>
    public string? OuId { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    public List<string> Roles { get; set; } = new();
}