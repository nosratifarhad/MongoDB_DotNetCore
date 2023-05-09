using ECommerce.Api.Domain.Entitys;

namespace ECommerce.Api.Domain;

public interface IProductWriteRepository
{
    Task<int> CreateProductAsync(Product product);

    Task UpdateProductAsync(Product product);

    Task DeleteProductAsync(int productId);
}
