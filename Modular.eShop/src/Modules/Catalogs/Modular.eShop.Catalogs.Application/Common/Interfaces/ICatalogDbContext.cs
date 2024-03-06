using Microsoft.EntityFrameworkCore;
using Modular.eShop.Catalogs.Domain.Entities;

namespace Modular.eShop.Catalogs.Application.Common.Interfaces;

public interface ICatalogDbContext
{
    DbSet<Product> Products { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
