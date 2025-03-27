using DoAnLTW_Nhom4.Repositories.EFRepositories;
using DoAnLTW_Nhom4.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace DoAnLTW_Nhom4.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IReviewRepository _reviewRepository;

        public ProductController(IProductRepository productRepository, IReviewRepository reviewRepository)
        {
            _productRepository = productRepository;
            _reviewRepository = reviewRepository;
        }

        // Hiển thị danh sách tất cả sản phẩm
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }
        // Hiển thị thông tin chi tiết sản phẩm

        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            product.Reviews = await _reviewRepository.GetReviewsByProductIdAsync(id);
            return View(product);
        }
        // Hiển thị sản phẩm theo danh mục
        public async Task<IActionResult> ByCategory(int categoryId)
        {
            var products = await _productRepository.GetByCategoryAsync(categoryId);
            return View("Index", products); // Sử dụng lại view Index
        }

        // Hiển thị sản phẩm theo thương hiệu
        public async Task<IActionResult> ByBrand(int brandId)
        {
            var products = await _productRepository.GetByBrandAsync(brandId);
            return View("Index", products); // Sử dụng lại view Index
        }
    }
}