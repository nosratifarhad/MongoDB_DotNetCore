using ECommerce.Api.Domain;
using ECommerce.Api.Domain.Entitys;
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

        #region [ Implement ]

        public async ValueTask<ProductViewModel> GetProductAsync(int productId)
        {
            if (productId <= 0)
                throw new NullReferenceException("Product Id Is Invalid");

            var cacheResult = await GetFromCacheAsync(productId.ToString());
            if (cacheResult != null)
                return cacheResult;

            var product = await _productReadRepository.GetProductAsync(productId).ConfigureAwait(false);
            if (product == null)
                return new ProductViewModel();

            var productViewModel = CreateProductViewModelFromProduct(product);

            await SetInToCacheAsync(productViewModel).ConfigureAwait(false);

            return productViewModel;
        }

        public async ValueTask<IEnumerable<ProductViewModel>> GetProductsAsync()
        {
            var cacheResult = await GetFromCacheAsync();
            if (cacheResult != null)
                return cacheResult;

            var products = await _productReadRepository.GetProductsAsync().ConfigureAwait(false);

            if (products == null || products.Count() == 0)
                return Enumerable.Empty<ProductViewModel>();

            var productViewModels = CreateProductViewModelsFromProducts(products);

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

        public async Task<List<ProductViewModel>> GetProductByFilterAsync(string id, string productName, string productTitle)
        {
            var cacheResults = await GetFromCacheAsyncFromCacheAsync(id, productName, productTitle);
            if (cacheResults != null)
                return cacheResults;

            var products = await _productReadRepository.GetProductByFilterAsync(id, productName, productTitle).ConfigureAwait(false);
            if (products == null)
                return new List<ProductViewModel>();

            var productViewModels = CreateProductViewModelFromProducts(products);

            await SetListInToCacheAsync(productViewModels).ConfigureAwait(false);

            return productViewModels;
        }

        #endregion [ Implement ]

        #region [ Cache Private Method ]

        private async Task SetInToCacheAsync(ProductViewModel result)
            => await _mongoCacheRepository
                 .CreateAsync(result);

        private async Task SetListInToCacheAsync(List<ProductViewModel> results)
            => await _mongoCacheRepository
                 .CreateAsync(results);

        private async Task SetManyInToCacheAsync(IEnumerable<ProductViewModel> results)
            => await _mongoCacheRepository
                 .CreateManyAsync(results);

        private async Task<ProductViewModel> GetFromCacheAsync(string id)
            => await _mongoCacheRepository
                .GetAsync(id);

        private async Task<List<ProductViewModel>> GetFromCacheAsyncFromCacheAsync(string id, string productName, string productTitle)
             => await _mongoCacheRepository
                   .GetProductByFilterAsync(id, productName, productTitle);

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

        private ProductViewModel CreateProductViewModelFromProduct(Product product)
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

        private List<ProductViewModel> CreateProductViewModelFromProducts(List<Product> products)
        {
            List<ProductViewModel> productsViewModels = new List<ProductViewModel>();
            foreach (var item in products)
            {
                productsViewModels.Add(new ProductViewModel()
                {
                    Id = item.ProductId.ToString(),
                    ProductName = item.ProductName,
                    ProductTitle = item.ProductTitle,
                    ProductDescription = item.ProductDescription,
                    MainImageName = item.MainImageName,
                    MainImageTitle = item.MainImageTitle,
                    MainImageUri = item.MainImageUri,
                    IsExisting = item.IsExisting,
                    IsFreeDelivery = item.IsFreeDelivery,
                    Weight = item.Weight
                });
            }
            return productsViewModels;
        }

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

        private IEnumerable<ProductViewModel> CreateProductViewModelsFromProducts(IEnumerable<Product> products)
        {
            ICollection<ProductViewModel> productViewModels = new List<ProductViewModel>();

            foreach (var product in products)
                productViewModels.Add(
                     new ProductViewModel()
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
