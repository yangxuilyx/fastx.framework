using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FastX.DependencyInjection;
using FastX.Identity.Core.Identity.Users;
using FastX.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FastX.Identity.Core.Identity;

/// <summary>
/// SignInManager
/// </summary>
public class SignInManager : ITransientDependency
{
    private readonly UserManager _userManager;

    private readonly IHttpContextAccessor _contextAccessor;
    private HttpContext? _context;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// The <see cref="HttpContext"/> used.
    /// </summary>
    public HttpContext Context
    {
        get
        {
            var context = _context ?? _contextAccessor?.HttpContext;
            if (context == null)
            {
                throw new InvalidOperationException("HttpContext must not be null.");
            }
            return context;
        }
        set
        {
            _context = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public SignInManager(UserManager userManager, IHttpContextAccessor contextAccessor, IConfiguration configuration)
    {
        _userManager = userManager;
        _contextAccessor = contextAccessor;
        _configuration = configuration;
    }

    /// <summary>
    /// Attempts a password sign in for a user.
    /// </summary>
    /// <param name="user">The user to sign in.</param>
    /// <param name="password">The password to attempt to sign in with.</param>
    /// <returns>The task object representing the asynchronous operation containing the <see name="SignInResult"/>
    /// for the sign-in attempt.</returns>
    /// <returns></returns>
    public virtual async Task<SignInResult> CheckPasswordSignInAsync(User user, string password)
    {
        if (await _userManager.CheckPasswordAsync(user, password))
            return SignInResult.Success;

        return SignInResult.Failed;
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="isPersistent"></param>
    /// <returns></returns>
    public virtual async Task<SignInResult> PasswordSignInAsync(string userName, string password,
        bool isPersistent)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return SignInResult.Failed;
        }

        var attempt = await CheckPasswordSignInAsync(user, password);
        return attempt.Succeeded
            ? await SignInAsync(user, isPersistent)
            : attempt;
    }

    /// <summary>
    /// SignInAsync
    /// </summary>
    /// <param name="user"></param>
    /// <param name="isPersistent"></param>
    /// <returns></returns>
    protected async Task<SignInResult> SignInAsync(User user, bool isPersistent)
    {
        var userPrincipal = await CreateUserPrincipalAsync(user);
        Context.User = userPrincipal;

        return SignInResult.Success;
    }

    /// <summary>
    /// CreateUserPrincipalAsync
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    protected Task<ClaimsPrincipal> CreateUserPrincipalAsync(User user)
    {
        var claimsIdentity = new ClaimsIdentity("Bearer",
            ClaimTypes.Name,
            ClaimTypes.Role);

        claimsIdentity.AddClaim(new Claim(XClaimTypes.UserId, user.UserId));
        claimsIdentity.AddClaim(new Claim(XClaimTypes.UserName, user.UserName));
        claimsIdentity.AddClaim(new Claim(XClaimTypes.Name, user.Name));

        return Task.FromResult(new ClaimsPrincipal(claimsIdentity));
    }

    /// <summary>
    /// 创建Token
    /// </summary>
    /// <param name="principal"></param>
    /// <param name="externalClaim"></param>
    /// <returns></returns>
    public async Task<TokenResult> CreateToken(ClaimsPrincipal principal, List<Claim>? externalClaim)
    {
        // 1. 定义需要使用到的Claims

        // 2. 从 appsettings.json 中读取SecretKey
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

        // 3. 选择加密算法
        var algorithm = SecurityAlgorithms.HmacSha256;

        // 4. 生成Credentials
        var signingCredentials = new SigningCredentials(secretKey, algorithm);

        var expires = new DateTimeOffset(DateTime.Now.AddYears(1));

        // 5. 根据以上，生成token
        var jwtSecurityToken = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],     //Issuer
            _configuration["Jwt:Audience"],   //Audience
            principal.Claims,                          //Claims,
            DateTime.Now,                    //notBefore
            expires.DateTime,    //expires
            signingCredentials               //Credentials
        );

        // 6. 将token变为string
        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return new TokenResult()
        {
            AccessToken = token,
            ExpireIn = expires.ToUnixTimeSeconds()
        };
    }
}