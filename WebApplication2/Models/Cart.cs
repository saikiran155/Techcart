using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Techcart.Models
{
    public class Cart
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;//Default

        public string? UserId { get; set; }

        public decimal TotalPrice => Price * Quantity;

        


    }
}
