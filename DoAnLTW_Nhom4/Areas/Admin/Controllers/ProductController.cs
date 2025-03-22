using DoAnLTW_Nhom4.Models;
using DoAnLTW_Nhom4.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace DoAnLTW_Nhom4.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IProductSpecificationRepository _productSpecificationRepository;

        public ProductController(IProductRepository productRepository, 
                                ICategoryRepository categoryRepository, 
                                IBrandRepository brandRepository, 
                                IProductSpecificationRepository productSpecificationRepository )
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _productSpecificationRepository = productSpecificationRepository;
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
            return View(product);
        }

        // Thêm sản phẩm
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var brands = await _brandRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.Brands = new SelectList(brands, "Id", "Name");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Add(ProductCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageUrl != null)
                {
                    if (!IsValidImage(model.ImageUrl))
                    {
                        ModelState.AddModelError("ImageUrl", "Hình ảnh không hợp lệ");
                        ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
                        ViewBag.Brands = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name");
                        return View(model);
                    }
                    model.Product.ImageUrl = await SaveImage(model.ImageUrl);
                }
                if (model.ImagesUrl != null && model.ImagesUrl.Count > 0)
                {
                    model.Product.ImagesUrl = new List<ProductImage>();
                    foreach (var image in model.ImagesUrl)
                    {
                        if (!IsValidImage(image))
                        {
                            ModelState.AddModelError("ImagesUrl", "Hình ảnh không hợp lệ");
                            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
                            ViewBag.Brands = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name");
                            return View(model);
                        }
                        var imageUrl = await SaveImage(image);
                        model.Product.ImagesUrl.Add(new ProductImage { ImageUrl = imageUrl });
                    }
                }
                await _productRepository.AddAsync(model.Product);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            ViewBag.Brands = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name");
            return View(model);
        }


        // Kiểm tra hình ảnh hợp lệ
        private bool IsValidImage(IFormFile file)
        {
            var validImageTypes = new[] { "image/jpeg", "image/png", "image/gif" };
            var maxFileSize = 1 * 1024 * 1024; // 1MB

            if (!validImageTypes.Contains(file.ContentType) || file.Length > maxFileSize)
            {
                return false;
            }
            return true;
        }

        // Lưu hình ảnh
        [Authorize(Roles = $"{SD.Role_Admin}")]
        private async Task<string> SaveImage(IFormFile ImageUrl)
        {
            var savePath = Path.Combine("wwwroot/images", ImageUrl.FileName); // Thay đổi đường dẫn theo cấu hình của bạn

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await ImageUrl.CopyToAsync(fileStream);
            }
            return "/images/" + ImageUrl.FileName; // Trả về đường dẫn tương đối
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