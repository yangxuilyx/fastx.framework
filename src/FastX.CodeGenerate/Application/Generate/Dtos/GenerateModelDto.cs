using System.ComponentModel.DataAnnotations;

namespace FastX.CodeGenerate.Application.Generate.Dtos;

/// <summary>
/// 生成模型
/// </summary>
public class GenerateModelDto
{
    /// <summary>
    /// 表Id
    /// </summary>
    public string GenerateModelId { get; set; }

    /// <summary>
    /// 实体名称
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    public string CamelCaseName => Name.ToCamelCase();

    /// <summary>
    /// 显示名称
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// 模块名称
    /// </summary>
    public string? ModuleName { get; set; }

    /// <summary>
    /// 模块显示名称
    /// </summary>
    public string? ModuleDisplayName { get; set; }

    /// <summary>
    /// 命名空间
    /// </summary>
    public string? Namespace { get; set; }

    /// <summary>
    /// 字段列表
    /// </summary>
    public List<ModelFieldDto> Fields { get; set; } = new();
}