namespace BTL_WEBDEV2025.Models
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
    }
}


