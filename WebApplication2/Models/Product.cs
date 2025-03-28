using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Techcart.Models
{
    public class Product
    {
        [Key]

        public int Id { get; set; }
        [Required(ErrorMessage ="Name of the Poroduct is required")]
        [Display(Name = "Product Name")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Price of the Poroduct is required")]
        [Display(Name = "Product Price")]
        public decimal Price { get; set; }


        [Required(ErrorMessage = "Description of the Poroduct is required")]

        [Display(Name = "Product Description")]
        public string Description { get; set; }

        
        public string ImagePath { get; set; }
    }
}
