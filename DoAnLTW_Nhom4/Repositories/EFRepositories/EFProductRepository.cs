using DoAnLTW_Nhom4.Models;
using DoAnLTW_Nhom4.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using DoAnLTW_Nhom4.Data;


namespace DoAnLTW_Nhom4.Repositories.EFRepositories
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ImageUrls)
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Brand)
                    .Include(p => p.ProductSpecifications)
                    .Include(p => p.ImageUrls)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy sản phẩm với ID: {id}");
                }

                return product;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Có lỗi xảy ra khi tìm kiếm sản phẩm: {ex.Message}");
            }
        }
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetBestSellersAsync(int count = 10)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ImageUrls)
                .Include(p => p.OrderDetails)
                .ToListAsync();

            return products
                .Where(p => p.IsBestSeller || (p.OrderDetails != null && p.OrderDetails.Any()))
                .OrderByDescending(p => p.OrderDetails != null ? p.OrderDetails.Count : 0)
                .Take(count);
        }

        public async Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count = 10)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ImageUrls)
                .Where(p => p.IsFeatured)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetLatestProductsAsync(int count = 10)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ImageUrls)
                .OrderByDescending(p => p.Id) // Assuming higher ID means newer product
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ImageUrls)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Product>> SearchAsync(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                return await _context.Products.Include(p => p.Category).ToListAsync();
            }

            searchString = searchString.Trim().ToLower(); // Loại bỏ khoảng trắng và chuẩn hóa chữ thường

            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Name.ToLower().Contains(searchString) ||
                            p.Description.ToLower().Contains(searchString))
                .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetByBrandAsync(int brandId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ImageUrls)
                .Where(p => p.BrandId == brandId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetSpecialOffersAsync(int count = 10)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ImageUrls)
                .Where(p => p.Discount > 0)
                .OrderByDescending(p => p.Discount)
                .Take(count)
                .ToListAsync();
        }
    }
}