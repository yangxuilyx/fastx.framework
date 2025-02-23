using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using FastX.CodeGenerate.Models.CodeGenerate;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Scriban;

namespace FastX.CodeGenerate.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CodeGenerateController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    /// <summary>
    /// 
    /// </summary>
    public CodeGenerateController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpGet]
    public async Task<string> GetEntities()
    {
        return await Task.FromResult("");
    }

    /// <summary>
    /// 生成代码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<GenerateInput> Generate(GenerateInput input)
    {
        var templateViewModels = TemplateViewModel.GetNormalViewModels(input);
        foreach (var templateViewModel in templateViewModels)
        {
            var templateText = await System.IO.File.ReadAllTextAsync(templateViewModel.TemplatePath);
            var tpl = Template.Parse(templateText);
            templateViewModel.GenerateCode = await tpl.RenderAsync(input);
        }

        foreach (var templateViewModel in templateViewModels)
        {
            var fileInfo = new FileInfo(templateViewModel.SavePath);
            if (!fileInfo.Directory!.Exists)
            {
                fileInfo.Directory.Create();
            }
            await System.IO.File.WriteAllTextAsync(templateViewModel.SavePath, templateViewModel.GenerateCode);
        }

        return input;
    }
}