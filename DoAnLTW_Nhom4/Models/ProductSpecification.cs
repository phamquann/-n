using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnLTW_Nhom4.Models
{
    public class ProductSpecification
    {
        public int Id { get; set; }

        [BindProperty]
        public string Key { get; set; } // VD: "Màn hình", "RAM"

        [BindProperty]
        public string Value { get; set; } // VD: "6.7 inch", "8GB"

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}