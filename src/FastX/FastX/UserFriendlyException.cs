namespace FastX;

/// <summary>
/// 用户异常
/// </summary>
public class UserFriendlyException : Exception
{
    /// <summary>
    /// Details
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// UserFriendlyException
    /// </summary>
    /// <param name="message"></param>
    /// <param name="details"></param>
    /// <param name="innerException"></param>
    public UserFriendlyException(
        string message,
        string? details = null,
        Exception? innerException = null
    )
        : base(message, innerException)
    {
        Details = details;
    }
}