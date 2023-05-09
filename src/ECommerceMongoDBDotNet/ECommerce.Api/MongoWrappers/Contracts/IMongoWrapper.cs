using ECommerce.Api.ViewModels.ProductViewModels;

namespace ECommerce.Api.MongoWrappers.Contracts;

public interface IMongoWrapper
{
    Task<List<ProductViewModel>> GetAsync();

    Task<ProductViewModel> GetAsync(int id);

    Task CreateAsync(ProductViewModel product);

    Task UpdateAsync(int id, ProductViewModel product);

    Task RemoveAsync(int id);

}
