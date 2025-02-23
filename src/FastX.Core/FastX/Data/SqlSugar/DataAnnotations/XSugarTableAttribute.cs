namespace FastX.Data.SqlSugar.DataAnnotations;

[AttributeUsage(AttributeTargets.Class)]
public class XSugarTableAttribute(string prefix) : Attribute
{
    public string Prefix { get; set; } = prefix;

    public string? TableName { get; set; }
}