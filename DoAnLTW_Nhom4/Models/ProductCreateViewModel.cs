namespace DoAnLTW_Nhom4.Models
{
    public class ProductCreateViewModel
    {
        public Product Product { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public List<IFormFile> ImagesUrl { get; set; } = new List<IFormFile>();
        public List<ProductSpecification> ProductSpecifications { get; set; } = new List<ProductSpecification>();
    }
}
