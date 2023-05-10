using ECommerce.Api.Domain.Entitys;

namespace ECommerce.Api.Domain;

public interface IProductReadRepository
{
    Task<Product> GetProductAsync(int productId);

    Task<IEnumerable<Product>> GetProductsAsync();

    Task<bool> IsExistProductAsync(int productId);
}
