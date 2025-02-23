namespace FastX.Identity.Core.Identity;

public class TokenResult
{
    /// <summary>
    /// token
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// RefreshToken
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public long ExpireIn { get; set; }
}