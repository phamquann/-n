using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DoAnLTW_Nhom4.Models;
using DoAnLTW_Nhom4.Repositories.Interfaces;

namespace DoAnLTW_Nhom4.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IBrandRepository brandRepository,
            ILogger<HomeController> logger)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Get latest products
                var latestProducts = await _productRepository.GetLatestProductsAsync(8);
                ViewBag.LatestProducts = latestProducts ?? new List<Product>();

                // Get best sellers
                var bestSellers = await _productRepository.GetBestSellersAsync(8);
                ViewBag.BestSellers = bestSellers ?? new List<Product>();

                // Get categories
                var categories = await _categoryRepository.GetAllAsync();
                ViewBag.Categories = categories ?? new List<Category>();

                // Get brands
                var brands = await _brandRepository.GetAllAsync();
                ViewBag.Brands = brands ?? new List<Brand>();

                return View();
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error occurred while loading home page data");
                
                // Initialize empty lists to prevent null reference exceptions
                ViewBag.LatestProducts = new List<Product>();
                ViewBag.BestSellers = new List<Product>();
                ViewBag.Categories = new List<Category>();
                ViewBag.Brands = new List<Brand>();
                
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

