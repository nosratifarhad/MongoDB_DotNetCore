using ECommerce.Api.ViewModels.ProductViewModels;
using ECommerce.Service.InputModels.ProductInputModels;

namespace ECommerce.Api.Services.Contract
{
    public interface IProductService
    {
        Task<int> CreateProductAsync(CreateProductInputModel inputModel);

        Task UpdateProductAsync(UpdateProductInputModel inputModel);

        Task DeleteProductAsync(int productId);

        ValueTask<ProductViewModel> GetProductAsync(int productId);

        Task<List<ProductViewModel>> GetProductByFilterAsync(string id, string productName, string productTitle);

        ValueTask<IEnumerable<ProductViewModel>> GetProductsAsync();

    }
}
