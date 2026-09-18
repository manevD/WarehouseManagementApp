namespace WarehouseManagement.Configuration
{
    public class ShopifyOptions
    {
        public bool SetShopify { get; set; }
        public string ClientId { get; set; } = string.Empty;

        public string ClientSecret { get; set; } = string.Empty;
    }
}