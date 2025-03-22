using DoAnLTW_Nhom4.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnLTW_Nhom4.Models
{
    public class ProductSpecification
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; } // VD: "Màn hình", "RAM"

        [Required]
        public string Value { get; set; } // VD: "6.7 inch", "8GB"

        public Product Product { get; set; }
    }
}