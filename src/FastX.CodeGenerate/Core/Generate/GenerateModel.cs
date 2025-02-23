using System.ComponentModel.DataAnnotations;
using FastX.Data.Entities;
using FastX.Data.SqlSugar.DataAnnotations;

namespace FastX.CodeGenerate.Core.Generate;

/// <summary>
/// 表
/// </summary>
[XSugarTable("Generate")]
public class GenerateModel : Entity
{
    /// <summary>
    /// 表Id
    /// </summary>
    public Ulid TableId { get; set; }

    /// <summary>
    /// 实体名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

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
}