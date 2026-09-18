namespace WarehouseManagement.Models
{
    public class ShopifyProductMapping
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ShopifyProductId { get; set; } = string.Empty;

        public string ShopifyVariantId { get; set; } = string.Empty;

        public string ShopifyInventoryItemId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}