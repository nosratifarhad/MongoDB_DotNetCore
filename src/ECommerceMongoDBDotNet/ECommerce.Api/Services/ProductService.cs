using ECommerce.Api.Domain;
using ECommerce.Api.Domain.Dtos;
using ECommerce.Api.Domain.Entitys;
using ECommerce.Api.Repositorys.CacheRepository.Contracts;
using ECommerce.Api.Services.Contract;
using ECommerce.Api.ViewModels.ProductViewModels;
using ECommerce.Service.InputModels.ProductInputModels;
using System.Globalization;

namespace ECommerce.Api.Services
{
    public class ProductService : IProductService
    {
        #region Fields
        private readonly IMongoCacheRepository _mongoCacheRepository;

        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;

        #endregion Fields

        #region Ctor

        public ProductService(IMongoCacheRepository mongoCacheRepository,
            IProductReadRepository productReadRepository,
            IProductWriteRepository productWriteRepository)
        {
            _mongoCacheRepository = mongoCacheRepository;
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        #endregion Ctor

        #region Implement

        public async ValueTask<ProductViewModel> GetProductAsync(int productId)
        {
            if (productId <= 0)
                throw new NullReferenceException("Product Id Is Invalid");

            var cacheResult = await GetFromCacheAsync(productId.ToString());
            if (cacheResult != null)
                return cacheResult;

            var productDto = await _productReadRepository.GetProductAsync(productId).ConfigureAwait(false);
            if (productDto == null)
                return new ProductViewModel();

            var productViewModel = CreateProductViewModelFromProductDto(productDto);

            await SetInToCacheAsync(productViewModel).ConfigureAwait(false);

            return productViewModel;
        }

        public async ValueTask<IEnumerable<ProductViewModel>> GetProductsAsync()
        {
            var cacheResult = await GetFromCacheAsync();
            if (cacheResult != null)
                return cacheResult;

            var productDtos = await _productReadRepository.GetProductsAsync().ConfigureAwait(false);

            if (productDtos == null || productDtos.Count() == 0)
                return Enumerable.Empty<ProductViewModel>();

            var productViewModels = CreateProductViewModelsFromProductDtos(productDtos);

            await SetManyInToCacheAsync(productViewModels);

            return productViewModels;
        }

        public async Task<int> CreateProductAsync(CreateProductInputModel inputModel)
        {
            if (inputModel == null)
                throw new NullReferenceException("Product Id Is Invalid");

            ValidateProductName(inputModel.ProductName);

            ValidateProductTitle(inputModel.ProductTitle);

            var productEntoty = CreateProductEntityFromInputModel(inputModel);

            int productId = await _productWriteRepository.CreateProductAsync(productEntoty).ConfigureAwait(false);

            productEntoty.setProductId(productId);

            var productViewModel = CreateProductViewModelFromProductEntity(productEntoty);

            await SetInToCacheAsync(productViewModel).ConfigureAwait(false);

            return productId;
        }

        public async Task UpdateProductAsync(UpdateProductInputModel inputModel)
        {
            if (inputModel.ProductId <= 0)
                throw new NullReferenceException("ProductId Is Invalid.");

            ValidateProductName(inputModel.ProductName);

            ValidateProductTitle(inputModel.ProductTitle);

            await IsExistProduct(inputModel.ProductId).ConfigureAwait(false);

            var productEntoty = CreateProductEntityFromInputModel(inputModel);

            await _productWriteRepository.UpdateProductAsync(productEntoty).ConfigureAwait(false);
            
            var productViewModel = CreateProductViewModelFromProductEntity(productEntoty);

            DeleteCacheAsync(productViewModel.Id);

            await SetInToCacheAsync(productViewModel).ConfigureAwait(false);
        }

        public async Task DeleteProductAsync(int productId)
        {
            if (productId <= 0)
                throw new NullReferenceException("ProductId Is Invalid.");

            await IsExistProduct(productId).ConfigureAwait(false);

            await _productWriteRepository.DeleteProductAsync(productId).ConfigureAwait(false);

            DeleteCacheAsync(productId.ToString());
        }

        #endregion Implement

        #region [ Cache Private Method ]

        private async Task SetInToCacheAsync(ProductViewModel result)
            => await _mongoCacheRepository
                 .CreateAsync(result);

        private async Task SetManyInToCacheAsync(IEnumerable<ProductViewModel> results)
            => await _mongoCacheRepository
                 .CreateManyAsync(results);

        private async Task<ProductViewModel> GetFromCacheAsync(string id)
            => await _mongoCacheRepository
                .GetAsync(id);

        private async Task<IEnumerable<ProductViewModel>> GetFromCacheAsync()
            => await _mongoCacheRepository
                .GetAsync();

        private async void DeleteCacheAsync(string id)
           => await _mongoCacheRepository.RemoveAsync(id);


        #endregion [ Cache Private Method ]

        #region Private

        private async Task IsExistProduct(int productId)
        {
            var isExistProduct = await _productReadRepository.IsExistProductAsync(productId).ConfigureAwait(false);
            if (isExistProduct == false)
                throw new NullReferenceException("ProductId Is Not Found.");
        }

        private Product CreateProductEntityFromInputModel(CreateProductInputModel inputModel)
            => new Product(inputModel.ProductName, inputModel.ProductTitle, inputModel.ProductDescription, inputModel.MainImageName, inputModel.MainImageTitle, inputModel.MainImageUri, inputModel.IsExisting, inputModel.IsFreeDelivery, inputModel.Weight);

        private Product CreateProductEntityFromInputModel(UpdateProductInputModel inputModel)
            => new Product(inputModel.ProductId, inputModel.ProductName, inputModel.ProductTitle, inputModel.ProductDescription, inputModel.MainImageName, inputModel.MainImageTitle, inputModel.MainImageUri, inputModel.IsExisting, inputModel.IsFreeDelivery, inputModel.Weight);

        private ProductViewModel CreateProductViewModelFromProductDto(ProductDto dto)
            => new ProductViewModel()
            {
                Id = dto.ProductId.ToString(),
                ProductName = dto.ProductName,
                ProductTitle = dto.ProductTitle,
                ProductDescription = dto.ProductDescription,
                MainImageName = dto.MainImageName,
                MainImageTitle = dto.MainImageTitle,
                MainImageUri = dto.MainImageUri,
                IsExisting = dto.IsExisting,
                IsFreeDelivery = dto.IsFreeDelivery,
                Weight = dto.Weight
            };

        private ProductViewModel CreateProductViewModelFromProductEntity(Product product)
            => new ProductViewModel()
            {
                Id = product.ProductId.ToString(),
                ProductName = product.ProductName,
                ProductTitle = product.ProductTitle,
                ProductDescription = product.ProductDescription,
                MainImageName = product.MainImageName,
                MainImageTitle = product.MainImageTitle,
                MainImageUri = product.MainImageUri,
                IsExisting = product.IsExisting,
                IsFreeDelivery = product.IsFreeDelivery,
                Weight = product.Weight
            };

        private IEnumerable<ProductViewModel> CreateProductViewModelsFromProductDtos(IEnumerable<ProductDto> dtos)
        {
            ICollection<ProductViewModel> productViewModels = new List<ProductViewModel>();

            foreach (var ProductDto in dtos)
                productViewModels.Add(
                     new ProductViewModel()
                     {
                         Id = ProductDto.ProductId.ToString(),
                         ProductName = ProductDto.ProductName,
                         ProductTitle = ProductDto.ProductTitle,
                         ProductDescription = ProductDto.ProductDescription,
                         MainImageName = ProductDto.MainImageName,
                         MainImageTitle = ProductDto.MainImageTitle,
                         MainImageUri = ProductDto.MainImageUri,
                         IsExisting = ProductDto.IsExisting,
                         IsFreeDelivery = ProductDto.IsFreeDelivery,
                         Weight = ProductDto.Weight
                     });


            return (IEnumerable<ProductViewModel>)productViewModels;
        }

        private void ValidateProductName(string productName)
        {
            if (string.IsNullOrEmpty(productName) || string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException(nameof(productName), "Product Name cannot be nul.l");
        }

        private void ValidateProductTitle(string productTitle)
        {
            if (string.IsNullOrEmpty(productTitle) || string.IsNullOrWhiteSpace(productTitle))
                throw new ArgumentException(nameof(productTitle), "Product Title cannot be null.");
        }


        #endregion Private
    }
}
