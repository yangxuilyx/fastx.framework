using System.Security.Claims;
using FastX.AspNetCore.Controllers;
using FastX.Identity.Core.Identity;
using FastX.Identity.Core.Identity.Users;
using FastX.Identity.Models.Account;
using Microsoft.AspNetCore.Mvc;

namespace FastX.Identity.Controllers;

/// <summary>
/// Account
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController : XController
{
    private readonly SignInManager _signInManager;
    private readonly UserManager _userManager;

    /// <summary>
    /// 
    /// </summary>
    public AccountController(SignInManager signInManager, UserManager userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    /// <summary>
    /// 安装系统
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<string> Setup([FromForm] string userName, [FromForm] string password)
    {
        var user = await _userManager.FindByNameAsync("admin");
        if (user != null)
            throw new Exception("请勿重复安装");

        await _userManager.CreateUserAsync(new User()
        {
            UserName = userName,
            Password = password,
            Name = "管理员",
            IsSpecial = true
        });

        return "安装成功";
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpPost]
    public async Task<TokenResult> Login(LoginModel input)
    {
        var signInResult = await _signInManager.PasswordSignInAsync(input.UserName, input.Password, input.IsPersistent);
        if (signInResult.Succeeded)
        {
            var claims = new List<Claim>();

            return await _signInManager.CreateToken(HttpContext.User, claims);
        }
        throw new Exception("登录失败");
    }

}