using ECommerce.Api.Domain.Dtos;

namespace ECommerce.Api.Domain;

public interface IProductReadRepository
{
    Task<ProductDto> GetProductAsync(int productId);

    Task<IEnumerable<ProductDto>> GetProductsAsync();

    Task<bool> IsExistProductAsync(int productId);
}
