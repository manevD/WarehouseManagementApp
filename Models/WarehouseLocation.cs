using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Models
{
    public class WarehouseLocation
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public int WarehouseId { get; set; }

        public Warehouse? Warehouse { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Stock> Stocks { get; set; }
            = new List<Stock>();
    }
}