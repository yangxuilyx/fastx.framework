namespace FastX.Application.Dtos;

public interface ISortedResultRequest
{
    /// <summary>
    /// 分页
    /// </summary>
    string? Sorting { get; set; }
}