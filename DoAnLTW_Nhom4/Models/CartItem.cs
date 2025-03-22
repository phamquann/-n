using DoAnLTW_Nhom4.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnLTW_Nhom4.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ShoppingCart")]
        public int ShoppingCartId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Quan hệ
        public ShoppingCart ShoppingCart { get; set; }
        public Product Product { get; set; }
    }
}