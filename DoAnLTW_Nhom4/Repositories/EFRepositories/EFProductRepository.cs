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
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ProductSpecifications)
                .Include(p => p.ImageUrls)
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new InvalidOperationException("Product not found");
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

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ImageUrls)
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
                .Where(p => p.BrandId == brandId)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ImageUrls)
                .ToListAsync();
        }
    }
}