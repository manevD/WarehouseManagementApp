using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Models
{
    public class Stock
    {
        public int Id { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal ReservedQuantity { get; set; }

        public decimal AvailableQuantity =>
            Quantity - ReservedQuantity;

        // Product
        public int ProductId { get; set; }

        public Product? Product { get; set; }

        // Lagerplatz
        public int WarehouseLocationId { get; set; }

        public WarehouseLocation? WarehouseLocation { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}