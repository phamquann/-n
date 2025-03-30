using DoAnLTW_Nhom4.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnLTW_Nhom4.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }  // Gán giỏ hàng theo từng user (nếu có đăng nhập)
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public int StockQuantity { get; set; }
        public bool IsSelected { get; set; } = false;

    }
}