using ECommerce.Api.Dtos;
using ECommerce.Api.Repositorys.ReadRepository;

namespace ECommerce.Api.Repositorys.Info
{
    public class ProductReadRepository : IProductReadRepository
    {
        public async Task<ProductDto> GetProduct(int productId)
        {
            await Task.Delay(100);
            return null;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            await Task.Delay(100);
            return null;
        }

        public async Task<bool> IsExistProduct(int productId)
        {
            await Task.Delay(100);
            return true;
        }
    }
}
