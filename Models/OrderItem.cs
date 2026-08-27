using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Models
{
    public class OrderItem
    {
        public int Id { get; set; }


        // =====================================================
        // ORDER
        // =====================================================

        public int OrderId { get; set; }

        public Order? Order { get; set; }


        // =====================================================
        // PRODUCT
        // =====================================================

        public int ProductId { get; set; }

        public Product? Product { get; set; }


        // =====================================================
        // QUANTITY
        // =====================================================

        [Range(
            0.01,
            double.MaxValue,
            ErrorMessage =
                "Die Menge muss größer als 0 sein.")]
        public decimal Quantity { get; set; }


        // =====================================================
        // PRICE
        // =====================================================

        [Range(
            0,
            double.MaxValue,
            ErrorMessage =
                "Der Preis darf nicht negativ sein.")]
        public decimal UnitPrice { get; set; }


        // =====================================================
        // TOTAL
        // =====================================================

        public decimal Total =>
            Quantity * UnitPrice;


        // =====================================================
        // OPTIONAL LOCATION
        // =====================================================

        public int? WarehouseLocationId { get; set; }

        public WarehouseLocation? WarehouseLocation { get; set; }
    }
}