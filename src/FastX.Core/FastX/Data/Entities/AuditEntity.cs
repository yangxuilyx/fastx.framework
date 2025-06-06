namespace FastX.Data.Entities;

public class AuditEntity : Entity, ISoftDelete, IHasCreateTime, IHasUpdateTime
{
    /// <summary>
    /// IsDeleted
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// CreateTime
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// UpdateTime
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}