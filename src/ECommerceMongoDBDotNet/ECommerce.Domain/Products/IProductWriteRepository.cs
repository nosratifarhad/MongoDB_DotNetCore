using ECommerce.Domain.Products.Entitys;

namespace ECommerce.Domain.Products
{
    public interface IProductWriteRepository
    {
        Task<int> CreateProductAsync(Product product);

        Task UpdateProductAsync(Product product);

        Task DeleteProductAsync(int productId);

    }
}
