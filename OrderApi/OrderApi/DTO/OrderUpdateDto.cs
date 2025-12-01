using System.ComponentModel.DataAnnotations;

namespace OrderApi.DTOs
{
    public class OrderUpdateDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
