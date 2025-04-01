using DoAnLTW_Nhom4.Data;
using DoAnLTW_Nhom4.Models;
using DoAnLTW_Nhom4.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DoAnLTW_Nhom4.Repositories.EFRepositories
{
    public class EFReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public EFReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách đánh giá theo sản phẩm
        public async Task<List<Review>> GetReviewsByProductIdAsync(int productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId && r.IsApproved)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        // Lấy đánh giá theo ID
        public async Task<Review?> GetReviewByIdAsync(int reviewId)
        {
            return await _context.Reviews.FindAsync(reviewId);
        }

        // Thêm đánh giá mới
        public async Task AddAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

        // Cập nhật đánh giá (duyệt hoặc chỉnh sửa)
        public async Task UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        // Xóa đánh giá
        public async Task DeleteAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
    }
}
