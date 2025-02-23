using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace FastX.AspNetCore.Conventions;

public class XConventionalControllerFeatureProvider : ControllerFeatureProvider
{
    /// <summary>
    /// Determines if a given <paramref name="typeInfo" /> is a controller.
    /// </summary>
    /// <param name="typeInfo">The <see cref="T:System.Reflection.TypeInfo" /> candidate.</param>
    /// <returns><see langword="true" /> if the type is a controller; otherwise <see langword="false" />.</returns>
    protected override bool IsController(TypeInfo typeInfo)
    {
        if (!typeof(IApplicationService).IsAssignableFrom(typeInfo))
            return false;

        if (!typeInfo.IsPublic
           || typeInfo.IsAbstract
           || typeInfo.IsGenericType
           )
            return false;

        return true;
    }
}