using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Name <= 10")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
