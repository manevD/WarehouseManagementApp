using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Models
{
    public class ShopifyConnection
    {
        public int Id { get; set; }

        // =========================================================
        // SHOP
        // =========================================================

        [Required]
        [MaxLength(200)]
        public string ShopDomain { get; set; } = string.Empty;


        // =========================================================
        // SHOPIFY SHOP NAME
        // =========================================================

        [MaxLength(200)]
        public string? ShopName { get; set; }


        // =========================================================
        // ACCESS TOKEN
        // =========================================================

        [MaxLength(2000)]
        public string? AccessToken { get; set; }


        // =========================================================
        // STATUS
        // =========================================================

        public bool IsActive { get; set; } = false;


        // =========================================================
        // SYNCHRONIZATION
        // =========================================================

        public DateTime? LastSyncAt { get; set; }

        [MaxLength(500)]
        public string? LastSyncError { get; set; }


        // =========================================================
        // DATES
        // =========================================================

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}