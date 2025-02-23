using FastX.CodeGenerate.Application.Generate.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FastX.CodeGenerate.Application.Generate;

/// <summary>
/// 生成服务
/// </summary>
public interface IGenerateAppService
{
    /// <summary>
    /// 生成代码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    Task<GenerateModelDto> Generate(GenerateModelDto input);
}