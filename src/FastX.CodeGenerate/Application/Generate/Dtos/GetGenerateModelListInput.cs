using FastX.Application.Dtos;
using FastX.Data.PagedResult;

namespace FastX.CodeGenerate.Application.Generate.Dtos;

public class GetGenerateModelListInput : IPagedResultRequest, ISortedResultRequest
{
    /// <summary>
    /// 模型名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Page
    /// </summary>
    public PageInfo Page { get; set; }

    /// <summary>
    /// 分页
    /// </summary>
    public string? Sorting { get; set; } = "GenerateModelId DESC";
}