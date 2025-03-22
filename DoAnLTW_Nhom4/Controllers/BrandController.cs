using DoAnLTW_Nhom4.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace YourProjectName.Controllers
{
    public class BrandController : Controller
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IProductRepository _productRepository;

        public BrandController(IBrandRepository brandRepository, IProductRepository productRepository)
        {
            _brandRepository = brandRepository;
            _productRepository = productRepository;
        }

        // Hiển thị danh sách thương hiệu
        public async Task<IActionResult> Index()
        {
            var brands = await _brandRepository.GetAllAsync();
            return View(brands);
        }

    }
}