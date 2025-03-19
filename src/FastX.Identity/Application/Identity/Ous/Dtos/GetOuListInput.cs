using FastX.Application.Dtos;

namespace FastX.Identity.Application.Identity.Ous.Dtos;

public class GetOuListInput : IPagedResultRequest
{
    /// <summary>
    /// 上级Id
    /// </summary>
    public string ParentId { get; set; }

    /// <summary>
    /// Page
    /// </summary>
    public PageInfo Page { get; set; }
}