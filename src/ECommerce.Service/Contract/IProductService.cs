using ECommerce.Service.InputModels.ProductInputModels;
using ECommerce.Service.ViewModels.ProductViewModels;

namespace ECommerce.Service.Contract
{
    public interface IProductService
    {
        Task<int> CreateProductAsync(CreateProductInputModel inputModel);

        Task UpdateProductAsync(UpdateProductInputModel inputModel);
        
        Task DeleteProductAsync(int productId);

        Task<ProductViewModel> GetProduct(int productId);

        Task<IEnumerable<ProductViewModel>> GetProducts();

    }
}
