using ECommerce.Api.Services.Contract;
using ECommerce.Service.InputModels.ProductInputModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        #region Fields
        private readonly IProductService _productService;
        #endregion Fields

        #region Ctor
        public ProductsController(IProductService productService)
        {
           this._productService = productService;
        }

        #endregion Ctor

        /// <summary>
        /// Get Product List
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/products")]
        public async Task<IActionResult> GetProducts()
        {
            var productVMs = await _productService.GetProductsAsync().ConfigureAwait(false);
            
            return Ok(productVMs);
        }

        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("/api/products")]
        public async Task<IActionResult> CreateProduct(CreateProductInputModel inputModel)
        {
            int productId = await _productService.CreateProductAsync(inputModel).ConfigureAwait(false);

            return CreatedAtRoute(nameof(GetProduct), new { productId = productId }, new { ProductId = productId });
        }

        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("/api/products/{productId:int}", Name = nameof(GetProduct))]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var product =await _productService.GetProductAsync(productId).ConfigureAwait(false);

            return Ok(product);
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("/api/products/{productId:int}")]
        public async Task<IActionResult> UpdateProduct(int productId, UpdateProductInputModel inputModel)
        {
            if (productId != inputModel.ProductId)
                return BadRequest();

            await _productService.UpdateProductAsync(inputModel).ConfigureAwait(false);

            return NoContent();
        }

        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("/api/products/{productId:int}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            await _productService.DeleteProductAsync(productId).ConfigureAwait(false);
            
            return NoContent();
        }

    }
}
