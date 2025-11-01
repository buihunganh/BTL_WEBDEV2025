namespace BTL_WEBDEV2025.Models
{
    public class ShoppingCartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        // Variant choices
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}

