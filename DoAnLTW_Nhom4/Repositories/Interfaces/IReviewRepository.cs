using DoAnLTW_Nhom4.Models;

namespace DoAnLTW_Nhom4.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetReviewsByProductIdAsync(int productId); // Lấy danh sách đánh giá theo ProductId
        Task<Review?> GetReviewByIdAsync(int reviewId); // Lấy 1 đánh giá theo Id
        Task AddAsync(Review review); // Thêm đánh giá mới
        Task UpdateAsync(Review review); // Cập nhật đánh giá
        Task DeleteAsync(int reviewId); // Xóa đánh giá
    }
}

