using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Models
{
    public class PurchaseOrderItem
    {
        public int Id { get; set; }


        // =========================================================
        // PURCHASE ORDER
        // =========================================================

        public int PurchaseOrderId { get; set; }

        public PurchaseOrder? PurchaseOrder { get; set; }


        // =========================================================
        // PRODUCT
        // =========================================================

        public int ProductId { get; set; }

        public Product? Product { get; set; }


        // =========================================================
        // QUANTITY
        // =========================================================

        [Range(
            0.01,
            double.MaxValue,
            ErrorMessage =
                "Die Menge muss größer als 0 sein.")]
        public decimal Quantity { get; set; }


        // =========================================================
        // UNIT PRICE
        // =========================================================

        [Range(
            0,
            double.MaxValue,
            ErrorMessage =
                "Der Preis darf nicht negativ sein.")]
        public decimal UnitPrice { get; set; }


        // =========================================================
        // TOTAL
        // =========================================================

        public decimal Total =>
            Quantity * UnitPrice;


        // =========================================================
        // DELIVERED QUANTITY
        // =========================================================

        public decimal DeliveredQuantity { get; set; }


        // =========================================================
        // REMAINING QUANTITY
        // =========================================================

        public decimal RemainingQuantity =>
            Math.Max(
                0,
                Quantity - DeliveredQuantity);
    }
}