namespace ECommerce.Api.Entitys
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductTitle { get; set; }

        public string? ProductDescription { get; set; }

        public string? MainImageName { get; set; }

        public string? MainImageTitle { get; set; }

        public string? MainImageUri { get; set; }

        public bool IsFreeDelivery { get; set; }

        public bool IsExisting { get; set; }

        public float? Price { get; set; }

        public float? OffPrice { get; set; }

        public int? Weight { get; set; }

        #region Ctor

        public Product(
            string productName,
            string productTitle,
            string? productDescription,
            string? mainImageName,
            string? mainImageTitle,
            string? mainImageUri,
            bool isFreeDelivery,
            bool isExisting,
            int? weight)
        {
            ProductName = productName;
            ProductTitle = productTitle;
            ProductDescription = productDescription;
            MainImageName = mainImageName;
            MainImageTitle = mainImageTitle;
            MainImageUri = mainImageUri;
            IsFreeDelivery = isFreeDelivery;
            IsExisting = isExisting;
            Weight = weight;
        }

        public Product(
            int productId,
            string productName,
            string productTitle,
            string? productDescription,
            string? mainImageName,
            string? mainImageTitle,
            string? mainImageUri,
            bool isFreeDelivery,
            bool isExisting,
            int? weight)
        {
            ProductId = productId;
            ProductName = productName;
            ProductTitle = productTitle;
            ProductDescription = productDescription;
            MainImageName = mainImageName;
            MainImageTitle = mainImageTitle;
            MainImageUri = mainImageUri;
            IsFreeDelivery = isFreeDelivery;
            IsExisting = isExisting;
            Weight = weight;
        }

        #endregion Ctor

    }
}
