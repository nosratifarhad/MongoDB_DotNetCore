using ECommerce.Domain.Products.Dtos.ProductDtos;

namespace ECommerce.Domain.Products
{
    public interface IProductReadRepository
    {
        Task<ProductDto> GetProduct(int productId);
        
        Task<IEnumerable<ProductDto>> GetProducts();

        Task<bool> IsExistProduct(int productId);

    }
}
