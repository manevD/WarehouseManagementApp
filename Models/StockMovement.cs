using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Models
{
    public class StockMovement
    {
        public int Id { get; set; }


        // =========================================================
        // PRODUCT
        // =========================================================

        public int ProductId { get; set; }

        public Product? Product { get; set; }


        // =========================================================
        // QUANTITY
        // =========================================================

        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }


        // =========================================================
        // MOVEMENT TYPE
        // =========================================================

        [Required]
        public StockMovementType Type { get; set; }


        // =========================================================
        // FROM LOCATION
        // =========================================================

        public int? FromWarehouseLocationId { get; set; }

        public WarehouseLocation? FromWarehouseLocation { get; set; }


        // =========================================================
        // TO LOCATION
        // =========================================================

        public int? ToWarehouseLocationId { get; set; }

        public WarehouseLocation? ToWarehouseLocation { get; set; }


        // =========================================================
        // SUPPLIER
        // =========================================================

        public int? SupplierId { get; set; }

        public Supplier? Supplier { get; set; }


        // =========================================================
        // PURCHASE ORDER
        // =========================================================

        public int? PurchaseOrderId { get; set; }

        public PurchaseOrder? PurchaseOrder { get; set; }


        // =========================================================
        // REFERENCE
        // =========================================================

        public string? ReferenceNumber { get; set; }


        // =========================================================
        // NOTE
        // =========================================================

        [MaxLength(500)]
        public string? Note { get; set; }


        // =========================================================
        // USER
        // =========================================================

        public string? UserId { get; set; }


        // =========================================================
        // DATE
        // =========================================================

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;
    }


    public enum StockMovementType
    {
        Wareneingang = 1,

        Warenausgang = 2,

        Umlagerung = 3,

        Inventur = 4,

        Korrektur = 5
    }
}