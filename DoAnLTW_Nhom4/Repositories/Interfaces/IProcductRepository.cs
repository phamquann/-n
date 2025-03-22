using DoAnLTW_Nhom4.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    }
}