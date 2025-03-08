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
public class AccountController : XController
{
    private readonly SignInManager _signInManager;

    /// <summary>
    /// 
    /// </summary>
    public AccountController(SignInManager signInManager)
    {
        _signInManager = signInManager;
    }


    /// <summary>
    /// Login
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

            return await _signInManager.CreateJwtToken(HttpContext.User, claims);
        }
        throw new Exception("登录失败");
    }
}