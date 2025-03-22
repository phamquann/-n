using DoAnLTW_Nhom4.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DoAnLTW_Nhom4.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        // Quan hệ
        public ICollection<Product>? Products { get; set; }
    }
}