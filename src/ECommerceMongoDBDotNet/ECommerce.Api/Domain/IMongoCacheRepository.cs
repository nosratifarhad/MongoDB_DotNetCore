using ECommerce.Api.ViewModels.ProductViewModels;

namespace ECommerce.Api.Domain;

public interface IMongoCacheRepository
{
    Task<List<ProductViewModel>> GetAsync();

    Task<ProductViewModel> GetAsync(string id);

    Task<List<ProductViewModel>> GetProductByFilterAsync(string id, string productName, string productTitle);

    Task CreateAsync(ProductViewModel product);

    Task CreateAsync(List<ProductViewModel> results);

    Task CreateManyAsync(IEnumerable<ProductViewModel> productViewModels);

    Task RemoveAsync(string id);
}
