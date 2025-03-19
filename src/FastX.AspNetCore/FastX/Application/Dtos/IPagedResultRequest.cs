namespace FastX.Application.Dtos;

public interface IPagedResultRequest
{
    /// <summary>
    /// 分页信息
    /// </summary>
    PageInfo Page { get; set; }
}