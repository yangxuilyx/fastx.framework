using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace FastX.AspNetCore.Conventions;

public interface IConventionalRouteBuilder
{
    string Build(
        string rootPath,
        string controllerName,
        ActionModel action,
        string httpMethod
    );
}
