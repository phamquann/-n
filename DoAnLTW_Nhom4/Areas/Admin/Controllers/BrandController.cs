using DoAnLTW_Nhom4.Models;
using DoAnLTW_Nhom4.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace DoAnLTW_Nhom4.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
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
        [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Employee}")]
        public async Task<IActionResult> Index()
        {
            var brands = await _brandRepository.GetAllAsync();
            return View(brands);
        }
        // Xem thoong tin thương hiệu
        [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Employee}")]
        public async Task<IActionResult> Display(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }
        // Thêm thương hiệu
        [Authorize(Roles = $"{SD.Role_Admin}")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = $"{SD.Role_Admin}")]
        public async Task<IActionResult> Add(Brand brand, IFormFile LogoUrl)
        {
            if (ModelState.IsValid)
            {
                if(LogoUrl != null)
                {
                    // luu anh
                    brand.LogoUrl = await SaveImage(LogoUrl);
                }
                await _brandRepository.AddAsync(brand);
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }
        // Sửa thông tin thương hiệu
        [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Employee}")]
        public async Task<IActionResult> Edit(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }
        [HttpPost]
        [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Employee}")]
        public async Task<IActionResult> Edit(int id, Brand brand, IFormFile LogoUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(brand);
            }

            try
            {
                var existingBrand = await _brandRepository.GetByIdAsync(id);
                if (existingBrand == null)
                {
                    return NotFound();
                }

                // Kiểm tra nếu có ảnh mới thì cập nhật, nếu không giữ ảnh cũ
                if (LogoUrl != null && LogoUrl.Length > 0)
                {
                    existingBrand.LogoUrl = await SaveImage(LogoUrl);
                }

                // Cập nhật thông tin thương hiệu
                existingBrand.Name = brand.Name;
                existingBrand.Description = brand.Description;

                await _brandRepository.UpdateAsync(existingBrand);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BrandExists(id))
                {
                    return NotFound();
                }
                throw;
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra, vui lòng thử lại");
                return View(brand);
            }
        }

        private async Task<bool> BrandExists(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            return brand != null;
        }

        // Xóa thương hiệu
        [Authorize(Roles = $"{SD.Role_Admin}")]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }
        [HttpPost,ActionName("DeleteConfirmed")]
        [Authorize(Roles = $"{SD.Role_Admin}")]
        public async Task<IActionResult> Delete(int id, Brand brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }
            await _brandRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
        // Ham luu hinh anh
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName); // Thay đổi đường dẫn theo cấu hình của bạn
             using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName; // Trả về đường dẫn tương đối
        }
    }
}