using ECommerce.Service.InputModels.ProductInputModels;
using ECommerce.Service.ViewModels.ProductViewModels;

namespace ECommerce.Api.MongoWrappers.Contracts
{
    public interface IMongoWrapper
    {
        Task<List<ProductModel>> GetAsync();

        Task<ProductModel?> GetAsync(string id);

        Task CreateAsync(ProductModel productViewModel);

        Task UpdateAsync(string id, ProductModel productViewModel);

        Task RemoveAsync(string id);

    }
}
