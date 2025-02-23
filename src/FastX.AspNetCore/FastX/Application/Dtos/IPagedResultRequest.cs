using FastX.Data.PagedResult;

namespace FastX.Application.Dtos;

public interface IPagedResultRequest
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public PageInfo Page { get; set; }
}