namespace FastX.Application.Dtos;

public class PageInfo
{
    /// <summary>
    /// 当前页
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// 分页大小
    /// </summary>
    public int PageSize { get; set; } = 1;

    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPage { get; set; }}