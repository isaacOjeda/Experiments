using MinimalApis01.Features.Products.Queries;

namespace MinimalApis01.Features.Products;

public static class ProductsEndpoints
{
    public static RouteGroupBuilder MapProductsApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetProductsQuery.GetProducts);

        return group;
    }
}