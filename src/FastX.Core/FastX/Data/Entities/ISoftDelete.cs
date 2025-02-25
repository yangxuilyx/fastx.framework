namespace FastX.Data.Entities;

public interface ISoftDelete
{
    /// <summary>
    /// IsDeleted
    /// </summary>
    bool IsDeleted { get; set; }
}