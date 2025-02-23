using FastX.Data.PagedResult;

namespace FastX.Application.Dtos;

public class PagedResultDto<T>
{
    /// <summary>
    /// 分页
    /// </summary>
    public PageInfo Page { get; set; } = new();

    /// <summary>
    /// ListData
    /// </summary>
    public List<T> Items { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    public PagedResultDto()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="page"></param>
    /// <param name="items"></param>
    public PagedResultDto(PageInfo page, List<T> items)
    {
        Page = page;
        Items = items;
    }
}