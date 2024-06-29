using System.ComponentModel.DataAnnotations;

namespace Shop_ProductsAPI.Models.Dto
{
    public class ProductDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
