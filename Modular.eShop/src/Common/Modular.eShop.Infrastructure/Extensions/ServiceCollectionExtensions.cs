using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modular.eShop.Infrastructure.Configuration;
using System.Reflection;

namespace Modular.eShop.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Installs the services using the <see cref="IServiceInstaller"/> implementations defined in the specified assemblies.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="assemblies">The assemblies to scan for service installer implementations.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection InstallServicesFromAssemblies(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        var serviceInstallers = InstanceFactory.CreateFromAssemblies<IServiceInstaller>(assemblies);
        foreach (var serviceInstaller in serviceInstallers)
        {
            serviceInstaller.Install(services, configuration);
        }

        return services;
    }

    /// <summary>
    /// Installs the modules using the <see cref="IModuleInstaller"/> implementations defined in the specified assemblies.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="assemblies">The assemblies to scan for module installer implementations.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection InstallModulesFromAssemblies(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        var moduleInstallers = InstanceFactory.CreateFromAssemblies<IModuleInstaller>(assemblies);

        foreach (var moduleInstaller in moduleInstallers)
        {
            moduleInstaller.Install(services, configuration);
        }

        return services;
    }
}
