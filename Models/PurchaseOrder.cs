using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }


        // =========================================================
        // BESTELLNUMMER
        // =========================================================

        [Required]
        [MaxLength(50)]
        public string OrderNumber { get; set; } = string.Empty;


        // =========================================================
        // SUPPLIER
        // =========================================================

        [Required]
        public int SupplierId { get; set; }

        public Supplier? Supplier { get; set; }


        // =========================================================
        // DATE
        // =========================================================

        public DateTime OrderDate { get; set; } =
            DateTime.UtcNow;


        // =========================================================
        // STATUS
        // =========================================================

        [Required]
        public PurchaseOrderStatus Status { get; set; } =
            PurchaseOrderStatus.Entwurf;


        // =========================================================
        // REFERENCE
        // =========================================================

        [MaxLength(100)]
        public string? ReferenceNumber { get; set; }


        // =========================================================
        // NOTE
        // =========================================================

        [MaxLength(500)]
        public string? Note { get; set; }


        // =========================================================
        // TOTAL
        // =========================================================

        public decimal Total { get; set; }


        // =========================================================
        // DATES
        // =========================================================

        public DateTime CreatedAt { get; set; } =
            DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }


        // =========================================================
        // ITEMS
        // =========================================================

        public ICollection<PurchaseOrderItem> Items { get; set; }
            = new List<PurchaseOrderItem>();
    }


    public enum PurchaseOrderStatus
    {
        Entwurf = 1,

        Bestellt = 2,

        TeilweiseGeliefert = 3,

        Geliefert = 4,

        Storniert = 5
    }
}