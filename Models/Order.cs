using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Entwurf;

        [MaxLength(100)]
        public string? ReferenceNumber { get; set; }

        [MaxLength(300)]
        public string? DeliveryStreet { get; set; }

        [MaxLength(20)]
        public string? DeliveryPostalCode { get; set; }

        [MaxLength(100)]
        public string? DeliveryCity { get; set; }

        [MaxLength(100)]
        public string? DeliveryCountry { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }

        public decimal Total { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<OrderItem> Items { get; set; }
            = new List<OrderItem>();
    }


    public enum OrderStatus
    {
        Entwurf = 1,

        Bestätigt = 2,

        InBearbeitung = 3,

        TeilweiseGeliefert = 4,

        Versendet = 5,

        Abgeschlossen = 6,

        Storniert = 7
    }
}