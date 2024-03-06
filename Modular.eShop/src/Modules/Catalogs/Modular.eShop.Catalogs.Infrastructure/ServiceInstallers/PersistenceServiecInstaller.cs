using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modular.eShop.Catalogs.Application.Common.Interfaces;
using Modular.eShop.Catalogs.Persistence;
using Modular.eShop.Infrastructure.Configuration;

namespace Modular.eShop.Catalogs.Infrastructure.ServiceInstallers;
internal class PersistenceServiecInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Catalogs"));
        });
        services.AddScoped<ICatalogDbContext>(services => services.GetRequiredService<CatalogDbContext>());
    }
}
