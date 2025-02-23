using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;

namespace FastX.AspNetCore.Conventions;

public class XServiceConventionWrapper : IApplicationModelConvention
{
    private readonly IServiceProvider _serviceProvider;
    public XServiceConventionWrapper(IServiceCollection services)
    {
        _serviceProvider = services.BuildServiceProvider();
    }

    public void Apply(ApplicationModel application)
    {
        _serviceProvider.GetRequiredService<IXServiceConvention>().Apply(application);
    }
}
