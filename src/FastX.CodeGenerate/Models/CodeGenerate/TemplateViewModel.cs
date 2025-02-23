namespace FastX.CodeGenerate.Models.CodeGenerate;

public class TemplateViewModel
{
    public string TemplatePath { get; set; } = string.Empty;

    public string GenerateCode { get; set; } = string.Empty;

    public string SavePath { get; set; } = string.Empty;

    public static List<TemplateViewModel> GetNormalViewModels(GenerateInput module)
    {
        var basePath = AppContext.BaseDirectory;

        var result = new List<TemplateViewModel>()
            {
                new()
                {
                    TemplatePath = Path.Combine(basePath,@"Templates\angular\entity", "entity.component.ts.sbn"),
                    SavePath = Path.Combine(basePath,$@"_GenerateCode\angular\{module.Name.ToCamelCase()}\",$"{module.Name.ToKebabCase()}.component.ts"),
                },
                new()
                {
                    TemplatePath = Path.Combine(basePath,@"Templates\angular\entity", "entity.component.html.sbn"),
                    SavePath = Path.Combine(basePath,$@"_GenerateCode\angular\{module.Name.ToCamelCase()}\",$"{module.Name.ToKebabCase()}.component.html"),
                },
                new()
                {
                    TemplatePath = Path.Combine(basePath,@"Templates\angular\entity", "entity.component.less.sbn"),
                    SavePath = Path.Combine(basePath,$@"_GenerateCode\angular\{module.Name.ToCamelCase()}\",$"{module.Name.ToKebabCase()}.component.less"),
                },
                new()
                {
                    TemplatePath = Path.Combine(basePath,@"Templates\angular\entity\entity-edit", "entity-edit.component.ts.sbn"),
                    SavePath = Path.Combine(basePath,$@"_GenerateCode\angular\{module.Name.ToCamelCase()}\{module.Name.ToCamelCase()}-edit",$"{module.Name.ToKebabCase()}-edit.component.ts"),
                },
                new()
                {
                    TemplatePath = Path.Combine(basePath,@"Templates\angular\entity\entity-edit", "entity-edit.component.html.sbn"),
                    SavePath = Path.Combine(basePath,$@"_GenerateCode\angular\{module.Name.ToCamelCase()}\{module.Name.ToCamelCase()}-edit",$"{module.Name.ToKebabCase()}-edit.component.html"),
                },
                new()
                {
                    TemplatePath = Path.Combine(basePath,@"Templates\angular\entity\entity-edit", "entity-edit.component.less.sbn"),
                    SavePath = Path.Combine(basePath,$@"_GenerateCode\angular\{module.Name.ToCamelCase()}\{module.Name.ToCamelCase()}-edit",$"{module.Name.ToKebabCase()}-edit.component.less"),
                },

                new()
                {
                    TemplatePath = Path.Combine(basePath,@"Templates\Module\Entities", "Entity.cs.sbn"),
                    SavePath = Path.Combine(basePath,$@"_GenerateCode\Module\{module.Name}s\",$"{module.Name}.cs"),
                },

                new()
                {
                    TemplatePath = Path.Combine(basePath,@"Templates\Module\Application\Entities", "IEntityAppService.cs.sbn"),
                    SavePath = Path.Combine(basePath,$@"_GenerateCode\Module\Application\{module.Name}s\",$"I{module.Name}AppService.cs"),
                },
                new()
                {
                    TemplatePath = Path.Combine(basePath,@"Templates\Module\Application\Entities", "EntityAppService.cs.sbn"),
                    SavePath = Path.Combine(basePath,$@"_GenerateCode\Module\Application\{module.Name}s\",$"{module.Name}AppService.cs"),
                },   
                new()
                {
                    TemplatePath = Path.Combine(basePath,@"Templates\Module\Application\Entities", "EntityProfile.cs.sbn"),
                    SavePath = Path.Combine(basePath,$@"_GenerateCode\Module\Application\{module.Name}s\",$"{module.Name}Profile.cs"),
                },
                new()
                {
                    TemplatePath = Path.Combine(basePath,@"Templates\Module\Application\Entities\Dtos", "CreateEntityDto.cs.sbn"),
                    SavePath = Path.Combine(basePath,$@"_GenerateCode\Module\Application\{module.Name}s\Dtos",$"Create{module.Name}Dto.cs"),
                },
                new()
                {
                    TemplatePath = Path.Combine(basePath,@"Templates\Module\Application\Entities\Dtos", "UpdateEntityDto.cs.sbn"),
                    SavePath = Path.Combine(basePath,$@"_GenerateCode\Module\Application\{module.Name}s\Dtos",$"Update{module.Name}Dto.cs"),
                },
                new()
                {
                    TemplatePath = Path.Combine(basePath,@"Templates\Module\Application\Entities\Dtos", "EntityDto.cs.sbn"),
                    SavePath = Path.Combine(basePath,$@"_GenerateCode\Module\Application\{module.Name}s\Dtos",$"{module.Name}Dto.cs"),
                },     
                new()
                {
                    TemplatePath = Path.Combine(basePath,@"Templates\Module\Application\Entities\Dtos", "GetEntityListInput.cs.sbn"),
                    SavePath = Path.Combine(basePath,$@"_GenerateCode\Module\Application\{module.Name}s\Dtos",$"Get{module.Name}ListInput.cs"),
                },
            };

        return result;
    }
}