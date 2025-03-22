using DoAnLTW_Nhom4.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnLTW_Nhom4.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}