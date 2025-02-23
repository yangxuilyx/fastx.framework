using System.Reflection;
using FastX.Application.Services;
using FastX.DependencyInjection;
using FastX.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FastX.AspNetCore.Conventions;

public class XServiceConvention : IXServiceConvention, ITransientDependency
{
    protected IConventionalRouteBuilder ConventionalRouteBuilder { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="conventionalRouteBuilder"></param>
    public XServiceConvention(IConventionalRouteBuilder conventionalRouteBuilder)
    {
        ConventionalRouteBuilder = conventionalRouteBuilder;
    }

    /// <summary>
    /// Called to apply the convention to the <see cref="T:Microsoft.AspNetCore.Mvc.ApplicationModels.ApplicationModel" />.
    /// </summary>
    /// <param name="application">The <see cref="T:Microsoft.AspNetCore.Mvc.ApplicationModels.ApplicationModel" />.</param>
    public void Apply(ApplicationModel application)
    {
        foreach (var controller in GetControllers(application))
        {
            var controllerType = controller.ControllerType.AsType();

            if (ImplementsRemoteServiceInterface(controllerType))
            {
                controller.ControllerName = controller.ControllerName.RemovePostFix(ApplicationService.CommonPostfixes);
                ConfigureRemoteService(controller);
            }
        }
    }

    protected virtual void ConfigureRemoteService(ControllerModel controller)
    {
        ConfigureApiExplorer(controller);
        ConfigureSelector(controller);
        ConfigureParameters(controller);
    }


    protected virtual void ConfigureApiExplorer(ControllerModel controller)
    {
        if (controller.ApiExplorer.IsVisible == null)
        {
            controller.ApiExplorer.IsVisible = true;
        }

        foreach (var action in controller.Actions)
        {
            ConfigureApiExplorer(action);
        }
    }

    protected virtual void ConfigureApiExplorer(ActionModel action)
    {
        if (action.ApiExplorer.IsVisible != null)
        {
            return;
        }

        action.ApiExplorer.IsVisible = true;
    }

    protected virtual void ConfigureSelector(ControllerModel controller)
    {
        RemoveEmptySelectors(controller.Selectors);

        var controllerType = controller.ControllerType.AsType();
        if (controller.Selectors.Any(selector => selector.AttributeRouteModel != null))
        {
            return;
        }

        var rootPath = GetRootPathOrDefault(controller.ControllerType.AsType());

        foreach (var action in controller.Actions)
        {
            ConfigureSelector(rootPath, controller.ControllerName, action);
        }
    }

    protected virtual void ConfigureSelector(string rootPath, string controllerName, ActionModel action)
    {
        RemoveEmptySelectors(action.Selectors);

        if (!action.Selectors.Any())
        {
            AddXServiceSelector(rootPath, controllerName, action);
        }
        else
        {
            NormalizeSelectorRoutes(rootPath, controllerName, action);
        }
    }

    protected virtual void NormalizeSelectorRoutes(string rootPath, string controllerName, ActionModel action)
    {
        foreach (var selector in action.Selectors)
        {
            var httpMethod = selector.ActionConstraints
                .OfType<HttpMethodActionConstraint>()
                .FirstOrDefault()?
                .HttpMethods?
                .FirstOrDefault();

            if (httpMethod == null)
            {
                httpMethod = SelectHttpMethod(action);
            }

            if (selector.AttributeRouteModel == null)
            {
                selector.AttributeRouteModel = CreateXServiceAttributeRouteModel(rootPath, controllerName, action, httpMethod);
            }

            if (!selector.ActionConstraints.OfType<HttpMethodActionConstraint>().Any())
            {
                selector.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { httpMethod }));
            }
        }
    }

    protected virtual void AddXServiceSelector(string rootPath, string controllerName, ActionModel action)
    {
        var httpMethod = SelectHttpMethod(action);

        var abpServiceSelectorModel = new SelectorModel
        {
            AttributeRouteModel = CreateXServiceAttributeRouteModel(rootPath, controllerName, action, httpMethod),
            ActionConstraints = { new HttpMethodActionConstraint(new[] { httpMethod }) }
        };

        action.Selectors.Add(abpServiceSelectorModel);
    }

    protected virtual AttributeRouteModel CreateXServiceAttributeRouteModel(string rootPath, string controllerName, ActionModel action, string httpMethod)
    {
        return new AttributeRouteModel(
            new RouteAttribute(
                ConventionalRouteBuilder.Build(rootPath, controllerName, action, httpMethod)
            )
        );
    }

    protected virtual string SelectHttpMethod(ActionModel action)
    {
        return HttpMethodHelper.GetConventionalVerbForMethodName(action.ActionName);
    }

    protected virtual string GetRootPathOrDefault(Type controllerType)
    {
        var areaAttribute = controllerType.GetCustomAttributes().OfType<AreaAttribute>().FirstOrDefault();
        if (areaAttribute?.RouteValue != null)
        {
            return areaAttribute.RouteValue;
        }

        return "app";
    }

    protected virtual void RemoveEmptySelectors(IList<SelectorModel> selectors)
    {
        selectors
            .Where(IsEmptySelector)
            .ToList()
            .ForEach(s => selectors.Remove(s));
    }

    protected virtual bool IsEmptySelector(SelectorModel selector)
    {
        return selector.AttributeRouteModel == null
               && selector.ActionConstraints.IsNullOrEmpty()
               && selector.EndpointMetadata.IsNullOrEmpty();
    }

    protected virtual bool ImplementsRemoteServiceInterface(Type controllerType)
    {
        return typeof(IApplicationService).GetTypeInfo().IsAssignableFrom(controllerType);
    }

    protected virtual IList<ControllerModel> GetControllers(ApplicationModel application)
    {
        return application.Controllers;
    }

    protected virtual void ConfigureParameters(ControllerModel controller)
    {
        /* Default binding system of Asp.Net Core for a parameter
         * 1. Form values
         * 2. Route values.
         * 3. Query string.
         */

        foreach (var action in controller.Actions)
        {
            foreach (var prm in action.Parameters)
            {
                if (prm.BindingInfo != null)
                {
                    continue;
                }

                if (!TypeHelper.IsPrimitiveExtended(prm.ParameterInfo.ParameterType, includeEnums: true))
                {
                    if (CanUseFormBodyBinding(action, prm))
                    {
                        prm.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
                    }
                }
            }
        }
    }

    protected virtual bool CanUseFormBodyBinding(ActionModel action, ParameterModel parameter)
    {
        //We want to use "id" as path parameter, not body!
        if (parameter.ParameterName == "id")
        {
            return false;
        }

        foreach (var selector in action.Selectors)
        {
            if (selector.ActionConstraints == null)
            {
                continue;
            }

            foreach (var actionConstraint in selector.ActionConstraints)
            {
                var httpMethodActionConstraint = actionConstraint as HttpMethodActionConstraint;
                if (httpMethodActionConstraint == null)
                {
                    continue;
                }

                if (httpMethodActionConstraint.HttpMethods.All(hm => hm.IsIn("GET", "DELETE", "TRACE", "HEAD")))
                {
                    return false;
                }
            }
        }

        return true;
    }
}