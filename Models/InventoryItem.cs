using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }

        // Inventory
        public int InventoryId { get; set; }

        public Inventory? Inventory { get; set; }


        // Product
        public int ProductId { get; set; }

        public Product? Product { get; set; }


        // Lagerplatz
        public int WarehouseLocationId { get; set; }

        public WarehouseLocation? WarehouseLocation { get; set; }


        // Bestand laut System
        [Range(0, double.MaxValue)]
        public decimal SystemQuantity { get; set; }


        // Physisch gezählt
        [Range(0, double.MaxValue)]
        public decimal? CountedQuantity { get; set; }
        // CountedQuantity - SystemQuantity
        public decimal Difference { get; set; }
    }
}