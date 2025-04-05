using DoAnLTW_Nhom4.Models;
using DoAnLTW_Nhom4.Repositories.EFRepositories;
using DoAnLTW_Nhom4.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace DoAnLTW_Nhom4.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IReviewRepository _reviewRepository;

        public ProductController(IProductRepository productRepository, IReviewRepository reviewRepository, ICategoryRepository categoryRepository, IBrandRepository brandRepository)
        {
            _productRepository = productRepository;
            _reviewRepository = reviewRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
        }

        // Hiển thị danh sách tất cả sản phẩm
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            // Get categories
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = categories ?? new List<Category>();

            // Get brands
            var brands = await _brandRepository.GetAllAsync();
            ViewBag.Brands = brands ?? new List<Brand>();
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> Filter(string search, int? categoryId, int? brandId, decimal? minPrice, decimal? maxPrice, string sortOrder, bool? inStock, bool? hasDiscount)
        {
            if (string.IsNullOrEmpty(search) && categoryId == null && minPrice == null && maxPrice == null && brandId == null && sortOrder == null && inStock == false && hasDiscount == false)
            {
                return PartialView("_NoProductsPartial");
            }
            var products = await _productRepository.GetFilteredProductsAsync(search, categoryId, brandId, minPrice, maxPrice, sortOrder, inStock, hasDiscount);
            //if (products.Any())
            //{
            //    return PartialView("_NoProductsPartial");
            //}
            return PartialView("_ProductListPartial", products); // Trả về danh sách sản phẩm cập nhật
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

        //Lazy loading
        public async Task<IActionResult> LoadMoreProducts(int page = 1, int pageSize = 6)
        {
            var paginatedProducts = await _productRepository.GetPaginatedProductsAsync(page, pageSize);

            if (!paginatedProducts.Any())
            {
                return Content("");
            }

            return PartialView("_ProductListPartial", paginatedProducts);
        }
    }
}