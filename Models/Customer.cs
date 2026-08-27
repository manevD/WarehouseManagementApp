using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Firmenname ist erforderlich.")]
        [MaxLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? ContactPerson { get; set; }

        [EmailAddress(ErrorMessage = "Ungültige E-Mail-Adresse.")]
        [MaxLength(200)]
        public string? Email { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }

        [MaxLength(300)]
        public string? Street { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(50)]
        public string? VatNumber { get; set; }

        [MaxLength(100)]
        public string? CustomerNumber { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}