using FastX.Application.Dtos;
using FastX.Data.PagedResult;

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