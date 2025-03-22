using DoAnLTW_Nhom4.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DoAnLTW_Nhom4.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CartId { get; set; } // Có thể dùng GUID để định danh giỏ hàng

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Quan hệ
        public ICollection<CartItem> CartItems { get; set; }
    }
}