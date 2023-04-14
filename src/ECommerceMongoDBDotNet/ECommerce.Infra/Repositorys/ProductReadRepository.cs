using Bogus;
using ECommerce.Domain.Products;
using ECommerce.Domain.Products.Dtos.ProductDtos;
using ECommerce.Domain.Products.Enums;

namespace ECommerce.Infra.Repositorys
{
    public class ProductReadRepository : IProductReadRepository
    {
        public async Task<ProductDto> GetProduct(int productId)
        {
            if (productId <= 5)
                return await Task.FromResult(CreateFakerProductDto());

            return await Task.FromResult(new ProductDto());
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            return await Task.FromResult((IEnumerable<ProductDto>)CreateFakerProductDtos());
        }

        public async Task<bool> IsExistProduct(int productId)
        {
            if (productId <= 5)
                return await Task.FromResult(true);

            return await Task.FromResult(false);
        }

        private static ProductDto CreateFakerProductDto()
           => new Faker<ProductDto>()
              .RuleFor(bp => bp.ProductId, f => f.Random.Number())
              .RuleFor(bp => bp.ProductName, f => f.Name.FirstName())
              .RuleFor(bp => bp.ProductTitle, f => f.Name.JobTitle())
              .RuleFor(bp => bp.ProductDescription, f => f.Name.JobDescriptor())
              .RuleFor(bp => bp.ProductCategory, f => f.Random.Enum<ProductCategory>())
              .RuleFor(bp => bp.MainImageName, f => f.Name.FullName())
              .RuleFor(bp => bp.MainImageTitle, f => f.Name.FullName())
              .RuleFor(bp => bp.MainImageUri, f => f.Name.FullName())
              .RuleFor(bp => bp.Color, f => f.Random.Enum<ProductColor>())
              .RuleFor(bp => bp.IsFreeDelivery, f => f.Random.Bool())
              .RuleFor(bp => bp.IsExisting, f => f.Random.Bool())
              .RuleFor(bp => bp.Weight, f => f.Random.Number());

        private static List<ProductDto> CreateFakerProductDtos()
           => new Faker<ProductDto>()
              .RuleFor(bp => bp.ProductId, f => f.Random.Number())
              .RuleFor(bp => bp.ProductName, f => f.Name.FirstName())
              .RuleFor(bp => bp.ProductTitle, f => f.Name.JobTitle())
              .RuleFor(bp => bp.ProductDescription, f => f.Name.JobDescriptor())
              .RuleFor(bp => bp.ProductCategory, f => f.Random.Enum<ProductCategory>())
              .RuleFor(bp => bp.MainImageName, f => f.Name.FullName())
              .RuleFor(bp => bp.MainImageTitle, f => f.Name.FullName())
              .RuleFor(bp => bp.MainImageUri, f => f.Name.FullName())
              .RuleFor(bp => bp.Color, f => f.Random.Enum<ProductColor>())
              .RuleFor(bp => bp.IsFreeDelivery, f => f.Random.Bool())
              .RuleFor(bp => bp.IsExisting, f => f.Random.Bool())
              .RuleFor(bp => bp.Weight, f => f.Random.Number()).Generate(5);

    }
}
