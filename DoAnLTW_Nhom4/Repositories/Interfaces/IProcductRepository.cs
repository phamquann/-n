using DoAnLTW_Nhom4.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DoAnLTW_Nhom4.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(); // Lấy tất cả sản phẩm
        Task<Product> GetByIdAsync(int id); // Lấy sản phẩm theo ID
        Task AddAsync(Product product); // Thêm sản phẩm
        Task UpdateAsync(Product product); // Cập nhật sản phẩm
        Task DeleteAsync(int id); // Xóa sản phẩm
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId); // Lấy sản phẩm theo danh mục
        Task<IEnumerable<Product>> GetByBrandAsync(int brandId);
        Task<IEnumerable<Product>> GetBestSellersAsync(int count = 10); // Lấy sản phẩm bán chạy nhất
        Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count = 10); // Lấy sản phẩm nổi bật
        Task<IEnumerable<Product>> GetLatestProductsAsync(int count = 10); // Lấy sản phẩm mới nhất
        Task<IEnumerable<Product>> GetSpecialOffersAsync(int count = 10);
        Task<IEnumerable<Product>> GetFilteredProductsAsync(string search, int? categoryId, int? brandId, decimal? minPrice, decimal? maxPrice, string sortOrder, bool? inStock, bool? hasDiscount);

    }
}