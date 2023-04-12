using Bogus;
using ECommerce.Domain.Products;
using ECommerce.Domain.Products.Entitys;

namespace ECommerce.Infra.Repositorys
{
    public class ProductWriteRepository : IProductWriteRepository
    {
        public async Task<int> CreateProductAsync(Product product)
        {
            await Task.Delay(1000);
            return await Task.FromResult(new Faker().Random.Number(1, 20));
        }

        public async Task DeleteProductAsync(int productId)
        {
            await Task.Delay(1000);
        }

        public async Task UpdateProductAsync(Product product)
        {
            await Task.Delay(1000);
        }
    }
}
