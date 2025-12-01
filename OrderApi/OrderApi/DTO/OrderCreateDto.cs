using System.ComponentModel.DataAnnotations;

namespace OrderApi.DTOs
{
    public class OrderCreateDto
    {
        [Required, MaxLength(200)]
        public string ProductName { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }
    }
}
