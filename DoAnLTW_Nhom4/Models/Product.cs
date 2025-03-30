using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnLTW_Nhom4.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm không được vượt quá 200 ký tự")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0")]
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        [Range(0, 100, ErrorMessage = "Phần trăm khuyến mãi phải lớn hơn 0")]
        public decimal? Discount { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        public int Stock { get; set; }

        public bool IsBestSeller { get; set; } = false;
        public bool IsFeatured { get; set; } = false;
        public List<ProductImage> ImageUrls { get; set; } = new List<ProductImage>();
        public List<ProductSpecification> ProductSpecifications { get; set; } = new List<ProductSpecification>();
        public List<Review>? Reviews { get; set; } = new List<Review>();
        public List<OrderDetail>? OrderDetails { get; set; }
        public List<CartItem>? CartItems { get; set; }
    }
}