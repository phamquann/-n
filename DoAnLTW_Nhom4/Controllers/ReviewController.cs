using DoAnLTW_Nhom4.Models;
using DoAnLTW_Nhom4.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DoAnLTW_Nhom4.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IProductRepository _productRepository;

        public ReviewController(IReviewRepository reviewRepository, IProductRepository productRepository)
        {
            _reviewRepository = reviewRepository;
            _productRepository = productRepository;
        }

        // Lấy danh sách đánh giá theo sản phẩm
        public async Task<IActionResult> GetReviews(int productId)
        {
            var reviews = await _reviewRepository.GetReviewsByProductIdAsync(productId);
            return PartialView("_ReviewList", reviews);
        }

        // Thêm đánh giá mới
        [HttpPost]
        public async Task<IActionResult> AddReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            review.CreatedAt = DateTime.Now;
            review.IsApproved = false; // Cần duyệt trước khi hiển thị
            await _reviewRepository.AddAsync(review);
            return Ok(new { message = "Đánh giá của bạn đã được gửi và đang chờ duyệt!" });
        }
    }
}
