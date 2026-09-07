using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Models
{
    public class Inventory
    {
        public int Id { get; set; }

        // Lager
        public int WarehouseId { get; set; }

        public Warehouse? Warehouse { get; set; }

        // Status
        [Required]
        public InventoryStatus Status { get; set; } =
            InventoryStatus.Entwurf;

        // Note
        [MaxLength(500)]
        public string? Note { get; set; }

        // User
        public string? UserId { get; set; }

        // Dates
        public DateTime CreatedAt { get; set; } =
            DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        // Items
        public ICollection<InventoryItem> Items { get; set; }
            = new List<InventoryItem>();
    }


    public enum InventoryStatus
    {
        Entwurf = 1,
        InBearbeitung = 2,
        Abgeschlossen = 3
    }
}