namespace Modular.eShop.Catalogs.Domain.Entities;

/// <summary>
/// Product entity.
/// </summary>
public class Product
{
    private Product()
    {
    }

    public ProductId Id { get; private set; }

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public decimal Price { get; private set; }

    public int ProductTypeId { get; private set; }

    public int ProductBrandId { get; private set; }

    public bool Active { get; set; }

    public ProductBrand? ProductBrand { get; private set; }

    public ProductType? ProductType { get; private set; }

    public static Product Create(ProductId id, string name, string description, decimal price, int productTypeId, int productBrandId)
    {
        var product = new Product
        {
            Id = id,
            Name = name,
            Description = description,
            Price = price,
            ProductTypeId = productTypeId,
            ProductBrandId = productBrandId,
        };

        return product;
    }
}
