using FastX.Application.Services;
using FastX.CodeGenerate.Application.Generate.Dtos;
using FastX.CodeGenerate.Core.Generate;
using FastX.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace FastX.CodeGenerate.Application.Generate;

/// <summary>
/// 生成服务
/// </summary>
public class GenerateAppService : ReadOnlyAppService<GenerateModel, string, GenerateModelDto, GetGenerateModelListInput>, IGenerateAppService
{
    private readonly IRepository<ModelField> _modelFieldRepository;

    /// <summary>
    /// 
    /// </summary>
    public GenerateAppService(IRepository<GenerateModel> repository, IRepository<ModelField> modelFieldRepository) : base(repository)
    {
        _modelFieldRepository = modelFieldRepository;
    }

    /// <summary>
    /// 生成代码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<GenerateModelDto> Generate(GenerateModelDto input)
    {
        var templateViewModels = Template.GetNormalViewModels(input);
        foreach (var templateViewModel in templateViewModels)
        {
            var templateText = await System.IO.File.ReadAllTextAsync(templateViewModel.TemplatePath);
            var tpl = Scriban.Template.Parse(templateText);
            templateViewModel.GenerateCode = await tpl.RenderAsync(input);
        }

        foreach (var templateViewModel in templateViewModels)
        {
            var fileInfo = new FileInfo(templateViewModel.SavePath);
            if (!fileInfo.Directory!.Exists)
            {
                fileInfo.Directory.Create();
            }
            await File.WriteAllTextAsync(templateViewModel.SavePath, templateViewModel.GenerateCode);
        }

        return await InsertOrUpdate(input);
    }

    protected override ISugarQueryable<GenerateModel> CreateFilteredQuery(GetGenerateModelListInput input)
    {
        return base.CreateFilteredQuery(input)
                .WhereIF(!input.Name.IsNullOrEmpty(),p=>p.Name.Contains(input.Name))
                .WhereIF(!input.DisplayName.IsNullOrEmpty(),p=>p.DisplayName != null && p.DisplayName.Contains(input.DisplayName))
            ;
    }

    protected override async Task<GenerateModelDto> MapToEntityDto(GenerateModel entity)
    {
        var dto = await base.MapToEntityDto(entity);

        var modelFields = await _modelFieldRepository.GetListAsync(p=>p.GenerateModelId == dto.GenerateModelId);
        dto.Fields = ObjectMapper.Map<List<ModelFieldDto>>(modelFields);

        return dto;
    }

    private async Task<GenerateModelDto> InsertOrUpdate(GenerateModelDto input)
    {
        var generateModel = await Repository.InsertOrUpdateAsync(ObjectMapper.Map<GenerateModelDto, GenerateModel>(input));

        foreach (var modelFieldDto in input.Fields)
        {
            var modelField = ObjectMapper.Map<ModelField>(modelFieldDto);
            modelField.GenerateModelId = generateModel.GenerateModelId;
            await _modelFieldRepository.InsertOrUpdateAsync(modelField);
        }

        return await GetAsync(generateModel.GenerateModelId);
    }
}