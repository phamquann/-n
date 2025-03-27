using Microsoft.AspNetCore.Mvc;
using DoAnLTW_Nhom4.Repositories.Interfaces;

namespace DoAnLTW_Nhom4.Components
{
    public class CarouselViewComponent : ViewComponent
    {
        private readonly IProductRepository _productRepository;

        public CarouselViewComponent(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var featuredProducts = await _productRepository.GetFeaturedProductsAsync(6);
            ViewBag.FeaturedProducts = featuredProducts;
            return View();
        }
    }
} 