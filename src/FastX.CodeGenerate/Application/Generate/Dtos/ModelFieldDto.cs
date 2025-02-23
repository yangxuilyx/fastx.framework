namespace FastX.CodeGenerate.Application.Generate.Dtos;

public class ModelFieldDto
{
    /// <summary>
    /// 字段Id
    /// </summary>
    public string ModelFieldId { get; set; }

    /// <summary>
    /// 字段名称
    /// </summary>
    public string? Name { get; set; }

    public string? CamelCaseName => Name?.ToCamelCase();

    /// <summary>
    /// 字段显示名称
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// 是否主键
    /// </summary>
    public bool IsPrimaryKey { get; set; }

    /// <summary>
    /// 是否可为空
    /// </summary>
    public bool CanBeNull { get; set; }

    /// <summary>
    /// 是否查询字段
    /// </summary>
    public bool IsQuery { get; set; }

    /// <summary>
    /// 是否列表字段
    /// </summary>
    public bool IsDto { get; set; }

    /// <summary>
    /// 是否创建
    /// </summary>
    public bool IsCreate { get; set; }

    /// <summary>
    /// 是否更新
    /// </summary>
    public bool IsUpdate { get; set; }
}