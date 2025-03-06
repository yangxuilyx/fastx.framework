using FastX.AspNetCore.Controllers;
using FastX.Data.Repository;
using FastX.Identity.Core.Identity.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FastX.Identity.Controllers;

/// <summary>
/// SetupController
/// </summary>
public class SetupController:XController
{
    private readonly IRepository<User> _userRepository;
    private readonly UserManager _userManager;

    /// <summary>
    /// 
    /// </summary>
    public SetupController(IRepository<User> userRepository, UserManager userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    /// <summary>
    /// 安装系统
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<string> Setup([FromForm] string userName, [FromForm] string password)
    {
        var user = await _userRepository.GetAsync(p=>true);
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
}