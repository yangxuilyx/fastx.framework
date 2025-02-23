using SqlSugar;

namespace Microsoft.Extensions.DependencyInjection;

public class XSugarRegistrationOptions
{
    public DbType DbType { get; set; }

    public string? ConnectionString { get; set; } = string.Empty;

    public bool IsAutoCloseConnection { get; set; } = true;
}