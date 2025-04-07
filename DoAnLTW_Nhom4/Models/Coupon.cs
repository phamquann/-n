using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnLTW_Nhom4.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Mã giảm giá không được để trống")]
        [StringLength(20, ErrorMessage = "Mã giảm giá không được vượt quá 20 ký tự")]
        [Display(Name = "Mã giảm giá")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Phần trăm giảm giá không được để trống")]
        [Range(0, 100, ErrorMessage = "Phần trăm giảm giá phải từ 0 đến 100")]
        [Display(Name = "Phần trăm giảm giá")]
        public decimal DiscountPercentage { get; set; }

        [Required(ErrorMessage = "Ngày hết hạn không được để trống")]
        [Display(Name = "Ngày hết hạn")]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Mô tả")]
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string? Description { get; set; }

        [Display(Name = "Số lần sử dụng")]
        public int UsageCount { get; set; } = 0;

        [Display(Name = "Giới hạn sử dụng")]
        public int? UsageLimit { get; set; }

        [Display(Name = "Giá trị đơn hàng tối thiểu")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinimumOrderAmount { get; set; }

        // Navigation property
        public virtual ICollection<Order> Orders { get; set; }

        public Coupon()
        {
            Orders = new HashSet<Order>();
        }
    }
} 