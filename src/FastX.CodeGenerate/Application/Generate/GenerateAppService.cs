using FastX.Application.Services;
using FastX.CodeGenerate.Application.Generate.Dtos;
using FastX.CodeGenerate.Core.Generate;
using FastX.Data.Repository;

namespace FastX.CodeGenerate.Application.Generate;

/// <summary>
/// 生成服务
/// </summary>
public class GenerateAppService : CrudAppService<GenerateModel,string, GenerateModelDto, GetGenerateModelListInput>, IGenerateAppService
{
    /// <summary>
    /// 
    /// </summary>
    public GenerateAppService(IRepository<GenerateModel> repository) : base(repository)
    {
    }
}