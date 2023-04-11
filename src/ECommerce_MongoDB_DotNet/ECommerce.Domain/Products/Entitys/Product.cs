using ECommerce.Domain.Products.Enums;

namespace ECommerce.Domain.Products.Entitys
{
    public class Product
    {
        public string ProductName { get; set; }

        public string ProductTitle { get; set; }

        public string ProductDescription { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public string MainImageName { get; set; }

        public string MainImageTitle { get; set; }

        public string MainImageUri { get; set; }

        public List<ProductImage> ImagesUri { get; set; }

        public ProductColor Color { get; set; }

        public bool IsFreeDelivery { get; set; }

        public float Price { get; set; }

        public float? OffPrice { get; set; }

        public List<Material> Materials { get; set; }

        public int Weight { get; set; }

        #region Ctor

        public Product(
            string productName,
            string productTitle,
            string productDescription,
            ProductCategory productCategory,
            string mainImageName,
            string mainImageTitle,
            string mainImageUri,
            List<ProductImage> imagesUri,
            ProductColor color,
            bool isFreeDelivery,
            float price,
            float offPrice,
            List<Material> materials,
            int weight)

        {
            ProductName = productName;
            ProductTitle = productTitle;
            ProductDescription = productDescription;
            ProductCategory = productCategory;
            MainImageName = mainImageName;
            MainImageTitle = mainImageTitle;
            MainImageUri = mainImageUri;
            ImagesUri = imagesUri;
            Color = color;
            IsFreeDelivery = isFreeDelivery;
            Price = price;
            Materials = materials;
            Weight = weight;
        }

        #endregion Ctor

        #region Builder

        public Product CreateProduct(
            string productName,
            string productTitle,
            string productDescription,
            ProductCategory productCategory,
            string mainImageName,
            string mainImageTitle,
            string mainImageUri,
            List<ProductImage> imagesUri,
            ProductColor color,
            bool isFreeDelivery,
            float price,
            float offPrice,
            List<Material> materials,
            int weight)
            => new Product(productName, productTitle, productDescription, productCategory,
                mainImageName, mainImageTitle, mainImageUri, imagesUri, color,
                isFreeDelivery, price, offPrice, materials, weight);

        #endregion
    }
}
