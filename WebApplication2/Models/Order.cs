using System;
using System.ComponentModel.DataAnnotations;

namespace Techcart.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public string UserId { get; set; } 

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        public string PaymentStatus { get; set; } = "Pending"; 

        public string? PaymentMethod { get; set; }

        public string OrderStatus { get; set; } = "Processing"; 
        public List<OrderItem> OrderItems { get; set; }
    }
}
