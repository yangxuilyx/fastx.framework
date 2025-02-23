using AutoMapper;
using FastX.CodeGenerate.Application.Generate.Dtos;
using FastX.CodeGenerate.Core.Generate;

namespace FastX.CodeGenerate;

/// <summary>
/// XApplicationAutoMapperProfile
/// </summary>
public class XCodeGenerateApplicationAutoMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public XCodeGenerateApplicationAutoMapperProfile()
    {
        CreateMap<GenerateModel, GenerateModelDto>().ReverseMap();
        CreateMap<ModelField, ModelFieldDto>().ReverseMap();
    }
}
