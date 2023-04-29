using ECommerce.Api.Dtos;
using ECommerce.Api.Entitys;
using ECommerce.Api.MongoWrappers.Contracts;
using ECommerce.Api.Repositorys.ReadRepository;
using ECommerce.Api.Repositorys.WriteRepository;
using ECommerce.Api.Services.Contract;
using ECommerce.Service.InputModels.ProductInputModels;
using ECommerce.Service.ViewModels.ProductViewModels;

namespace ECommerce.Api.Services
{
    public class ProductService : IProductService
    {
        #region Fields
        private readonly IMongoWrapper _mongoWrapper;

        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;

        #endregion Fields

        #region Ctor

        public ProductService(IMongoWrapper mongoWrapper,
            IProductReadRepository productReadRepository, 
            IProductWriteRepository productWriteRepository)
        {
            _mongoWrapper = mongoWrapper;
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        #endregion Ctor

        #region Implement

        public async Task<ProductModel> GetProduct(int productId)
        {

            var mongoProductDto = await _mongoWrapper.GetAsync(productId.ToString()).ConfigureAwait(false);

            var productDto = await _productReadRepository.GetProduct(productId).ConfigureAwait(false);

            var productViewModel = CreateProductViewModelFromProductDto(productDto);

            return productViewModel;
        }

        public async Task<IEnumerable<ProductModel>> GetProducts()
        {
            var productDtos = await _productReadRepository.GetProducts().ConfigureAwait(false);
            if (productDtos == null || productDtos.Count() == 0)
                return Enumerable.Empty<ProductModel>();

            var mongoProductDtos = await _mongoWrapper.GetAsync().ConfigureAwait(false);

            var productViewModels = CreateProductViewModelsFromProductDtos(productDtos);

            return productViewModels;
        }

        public async Task<int> CreateProductAsync(CreateProductInputModel inputModel)
        {
            ValidateProductName(inputModel.ProductName);

            ValidateProductTitle(inputModel.ProductTitle);

            var productEntoty = CreateProductEntityFromInputModel(inputModel);

            var mongoModel = ToProductModel(inputModel);

            await _mongoWrapper.CreateAsync(mongoModel).ConfigureAwait(false);

            return await _productWriteRepository.CreateProductAsync(productEntoty).ConfigureAwait(false);
        }

        public async Task UpdateProductAsync(UpdateProductInputModel inputModel)
        {
            ValidateProductName(inputModel.ProductName);

            ValidateProductTitle(inputModel.ProductTitle);

            await IsExistProduct(int.Parse(inputModel.ProductId)).ConfigureAwait(false);

            var productEntoty = CreateProductEntityFromInputModel(inputModel);

            var mongoModel = ToProductModel(inputModel);

            await _mongoWrapper.UpdateAsync(inputModel.ProductId.ToString(), mongoModel).ConfigureAwait(false);

            await _productWriteRepository.UpdateProductAsync(productEntoty).ConfigureAwait(false);
        }

        public async Task DeleteProductAsync(int productId)
        {
            await _mongoWrapper.RemoveAsync(productId.ToString());

            await _productWriteRepository.DeleteProductAsync(productId).ConfigureAwait(false);
        }

        #endregion Implement

        #region Private

        private async Task IsExistProduct(int productId)
        {
            var isExistProduct = await _productReadRepository.IsExistProduct(productId).ConfigureAwait(false);
            if (isExistProduct == false)
                throw new Exception("productId Is Not Found.");
        }

        private Product CreateProductEntityFromInputModel(CreateProductInputModel inputModel)
            => new Product(inputModel.ProductName, inputModel.ProductTitle, inputModel.ProductDescription, inputModel.MainImageName, inputModel.MainImageTitle, inputModel.MainImageUri, inputModel.IsExisting, inputModel.IsFreeDelivery, inputModel.Weight);

        private Product CreateProductEntityFromInputModel(UpdateProductInputModel inputModel)
            => new Product(int.Parse(inputModel.ProductId), inputModel.ProductName, inputModel.ProductTitle, inputModel.ProductDescription, inputModel.MainImageName, inputModel.MainImageTitle, inputModel.MainImageUri, inputModel.IsExisting, inputModel.IsFreeDelivery, inputModel.Weight);

        private ProductModel CreateProductViewModelFromProductDto(ProductDto dto)
            => new ProductModel()
            {
                ProductId = dto.ProductId.ToString(),
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

        private IEnumerable<ProductModel> CreateProductViewModelsFromProductDtos(IEnumerable<ProductDto> dtos)
        {
            ICollection<ProductModel> productViewModels = new List<ProductModel>();

            foreach (var ProductDto in dtos)
                productViewModels.Add(
                     new ProductModel()
                     {

                         ProductId = ProductDto.ProductId.ToString(),
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


            return productViewModels;
        }

        private void ValidateProductName(string productName)
        {
            if (string.IsNullOrEmpty(productName) || string.IsNullOrWhiteSpace(productName))
                throw new ArgumentNullException(nameof(productName), "Product Name must not be empty");
        }

        private void ValidateProductTitle(string productTitle)
        {
            if (string.IsNullOrEmpty(productTitle) || string.IsNullOrWhiteSpace(productTitle))
                throw new ArgumentNullException(nameof(productTitle), "Product Title must not be empty");
        }


        private ProductModel ToProductModel(UpdateProductInputModel inputModel)
            => new ProductModel()
            {
                ProductId = inputModel.ProductId,
                ProductName = inputModel.ProductName,
                ProductTitle = inputModel.ProductTitle,
                ProductDescription = inputModel.ProductDescription,
                MainImageName = inputModel.MainImageName,
                MainImageTitle = inputModel.MainImageTitle,
                MainImageUri = inputModel.MainImageUri,
                IsExisting = inputModel.IsExisting,
                IsFreeDelivery = inputModel.IsFreeDelivery,
                Weight = inputModel.Weight

            };

        private ProductModel ToProductModel(CreateProductInputModel inputModel)
            => new ProductModel()
            {
                ProductName = inputModel.ProductName,
                ProductTitle = inputModel.ProductTitle,
                ProductDescription = inputModel.ProductDescription,
                MainImageName = inputModel.MainImageName,
                MainImageTitle = inputModel.MainImageTitle,
                MainImageUri = inputModel.MainImageUri,
                IsExisting = inputModel.IsExisting,
                IsFreeDelivery = inputModel.IsFreeDelivery,
            };


        #endregion Private
    }
}
