using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderApi.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string ProductName { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        // Computed in app logic before save
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // Foreign key linking order to user
        [Required]
        public string UserId { get; set; } = string.Empty;
    }
}


