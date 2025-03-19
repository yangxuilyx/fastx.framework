using FastX.Application.Dtos;

namespace FastX.Identity.Application.Identity.Roles.Dtos;

/// <summary>
/// GetRoleListInput
/// </summary>
public class GetRoleListInput : IPagedResultRequest
{
    /// <summary>
    /// Page
    /// </summary>
    public PageInfo Page { get; set; }
}