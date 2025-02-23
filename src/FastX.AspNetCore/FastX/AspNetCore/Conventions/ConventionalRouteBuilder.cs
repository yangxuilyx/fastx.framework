using System.Reflection;
using FastX.DependencyInjection;
using FastX.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;

namespace FastX.AspNetCore.Conventions;

public class ConventionalRouteBuilder : IConventionalRouteBuilder, ITransientDependency
{
    public virtual string Build(
        string rootPath,
        string controllerName,
        ActionModel action,
        string httpMethod)
    {
        var apiRoutePrefix = GetApiRoutePrefix(action);
        var controllerNameInUrl =
            NormalizeUrlControllerName(rootPath, controllerName, action, httpMethod);

        var url = $"{apiRoutePrefix}/{rootPath}/{NormalizeControllerNameCase(controllerNameInUrl)}";

        //Add {id} path if needed
        //var idParameterModel = action.Parameters.FirstOrDefault(p => p.ParameterName == "id");
        //if (idParameterModel != null)
        //{
        //    if (TypeHelper.IsPrimitiveExtended(idParameterModel.ParameterType, includeEnums: true))
        //    {
        //        url += "/{id}";
        //    }
        //    else
        //    {
        //        var properties = idParameterModel
        //            .ParameterType
        //            .GetProperties(BindingFlags.Instance | BindingFlags.Public);

        //        foreach (var property in properties)
        //        {
        //            url += "/{" + NormalizeIdPropertyNameCase(property) + "}";
        //        }
        //    }
        //}

        //Add action name if needed
        var actionNameInUrl = NormalizeUrlActionName(rootPath, controllerName, action, httpMethod);
        if (!actionNameInUrl.IsNullOrEmpty())
        {
            url += $"/{NormalizeActionNameCase(actionNameInUrl)}";

            ////Add secondary Id
            //var secondaryIds = action.Parameters
            //    .Where(p => p.ParameterName.EndsWith("Id", StringComparison.Ordinal)).ToList();
            //if (secondaryIds.Count == 1)
            //{
            //    url += $"/{{{NormalizeSecondaryIdNameCase(secondaryIds[0])}}}";
            //}
        }

        return url;
    }

    protected virtual string GetApiRoutePrefix(ActionModel actionModel)
    {
        return "api";
    }

    protected virtual string NormalizeUrlActionName(string rootPath, string controllerName, ActionModel action,
        string httpMethod)
    {
        return HttpMethodHelper
            .RemoveHttpMethodPrefix(action.ActionName, httpMethod)
            .RemovePostFix("Async");
    }

    protected virtual string NormalizeUrlControllerName(string rootPath, string controllerName, ActionModel action,
        string httpMethod)
    {
        return controllerName;
    }

    protected virtual string NormalizeControllerNameCase(string controllerName)
    {
        return controllerName.ToCamelCase();
    }

    protected virtual string NormalizeActionNameCase(string actionName)
    {
        return actionName.ToCamelCase();
    }

    protected virtual string NormalizeIdPropertyNameCase(PropertyInfo property)
    {
        return property.Name;
    }

    protected virtual string NormalizeSecondaryIdNameCase(ParameterModel secondaryId)
    {
        return secondaryId.ParameterName;
    }
}