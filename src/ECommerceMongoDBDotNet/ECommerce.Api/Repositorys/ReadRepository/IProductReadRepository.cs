using ECommerce.Api.Dtos;

namespace ECommerce.Api.Repositorys.ReadRepository
{
    public interface IProductReadRepository
    {
        Task<ProductDto> GetProduct(int productId);

        Task<IEnumerable<ProductDto>> GetProducts();

        Task<bool> IsExistProduct(int productId);

    }
}
