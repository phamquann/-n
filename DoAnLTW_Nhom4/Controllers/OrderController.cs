using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DoAnLTW_Nhom4.Models;
using DoAnLTW_Nhom4.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

[Authorize] // Yêu cầu đăng nhập để truy cập
public class OrderController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderController(IOrderRepository orderRepository, UserManager<ApplicationUser> userManager)
    {
        _orderRepository = orderRepository;
        _userManager = userManager;
    }

    // 📌 Hiển thị danh sách đơn hàng của người dùng
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy ID của người dùng hiện tại
        var orders = await _orderRepository.GetAllAsync();

        // Lọc đơn hàng của người dùng hiện tại
        var userOrders = orders.Where(o => o.UserId == userId)
                               .OrderByDescending(o => o.OrderDate);

        return View(userOrders);
    }

    // 📌 Xem chi tiết đơn hàng
    public async Task<IActionResult> Display(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        // Kiểm tra xem đơn hàng có thuộc về người dùng hiện tại không
        if (order == null || order.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            TempData["ErrorMessage"] = "Đơn hàng không tồn tại hoặc bạn không có quyền truy cập!";
            return RedirectToAction("MyOrders");
        }

        return View(order);
    }

    // 📌 Hủy đơn hàng (chỉ cho phép khi đơn hàng chưa được xử lý)
    public async Task<IActionResult> CancelOrder(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null || order.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            TempData["ErrorMessage"] = "Đơn hàng không tồn tại hoặc bạn không có quyền truy cập!";
            return RedirectToAction("MyOrders");
        }

        if (order.Status != OrderStatus.Pending)
        {
            TempData["ErrorMessage"] = "Bạn không thể hủy đơn hàng này!";
            return RedirectToAction("MyOrders");
        }

        order.Status = OrderStatus.Cancelled;
        await _orderRepository.UpdateAsync(order);

        TempData["SuccessMessage"] = "Đơn hàng đã được hủy thành công!";
        return RedirectToAction("Index");
    }
}
