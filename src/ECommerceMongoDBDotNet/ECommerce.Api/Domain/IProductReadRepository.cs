using ECommerce.Api.Domain.Entitys;
using ECommerce.Api.ViewModels.ProductViewModels;

namespace ECommerce.Api.Domain;

public interface IProductReadRepository
{
    Task<Product> GetProductAsync(int productId);

    Task<List<Product>> GetProductByFilterAsync(string id, string productName, string productTitle);

    Task<IEnumerable<Product>> GetProductsAsync();

    Task<bool> IsExistProductAsync(int productId);
}
