using DoAnLTW_Nhom4.Extensions;
using DoAnLTW_Nhom4.Models;
using DoAnLTW_Nhom4.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnLTW_Nhom4.Data;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace DoAnLTW_Nhom4.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ShoppingCartController> _logger;

        public ShoppingCartController(
            IProductRepository productRepository, 
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            ILogger<ShoppingCartController> logger)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            try
            {
                _logger.LogInformation($"Adding product {productId} to cart with quantity {quantity}");
                
                if (productId <= 0)
                {
                    _logger.LogWarning($"Invalid product ID: {productId}");
                    return Json(new { success = false, message = "ID sản phẩm không hợp lệ" });
                }

                if (quantity <= 0)
                {
                    _logger.LogWarning($"Invalid quantity: {quantity}");
                    return Json(new { success = false, message = "Số lượng sản phẩm không hợp lệ" });
                }

                var product = await GetProductFromDatabase(productId);
                
                if (product == null)
                {
                    _logger.LogWarning($"Product not found: {productId}");
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
                }

                if (product.Stock < quantity)
                {
                    _logger.LogWarning($"Insufficient stock for product {productId}. Available: {product.Stock}, Requested: {quantity}");
                    return Json(new { success = false, message = $"Số lượng sản phẩm trong kho không đủ. Còn lại: {product.Stock}" });
                }

                var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
                var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

                if (cartItem != null)
                {
                    if (product.Stock < cartItem.Quantity + quantity)
                    {
                        _logger.LogWarning($"Insufficient stock for product {productId} when updating cart. Available: {product.Stock}, Current: {cartItem.Quantity}, Adding: {quantity}");
                        return Json(new { success = false, message = $"Số lượng sản phẩm trong kho không đủ. Còn lại: {product.Stock}" });
                    }
                    cartItem.Quantity += quantity;
                }
                else
                {
                    cart.Items.Add(new CartItem
                    {
                        ProductId = productId,
                        Name = product.Name,
                        ImageUrl = product.ImageUrl ?? string.Empty,
                        Price = (product.Price * (1 -product.Discount/100)),
                        Quantity = quantity,
                        StockQuantity = product.Stock
                    });
                }

                HttpContext.Session.SetObjectAsJson("Cart", cart);
                _logger.LogInformation($"Successfully added product {productId} to cart with quantity {quantity}");
                
                return Json(new { success = true, message = "Đã thêm sản phẩm vào giỏ hàng" });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, $"Error adding product {productId} to cart: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error adding product {productId} to cart: {ex.Message}");
                return Json(new { success = false, message = "Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng" });
            }
        }

        [Authorize]
        [Authorize(Roles = $"{SD.Role_Customer}")]
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            return View(cart);
        }

        private async Task<Product> GetProductFromDatabase(int productId)
        {
            try
            {
                _logger.LogInformation($"Attempting to find product with ID: {productId}");
                
                // Kiểm tra xem productId có hợp lệ không
                if (productId <= 0)
                {
                    _logger.LogWarning($"Invalid product ID: {productId}");
                    throw new InvalidOperationException($"ID sản phẩm không hợp lệ: {productId}");
                }

                // Sử dụng repository để lấy sản phẩm
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning($"Product not found with ID: {productId}");
                    throw new InvalidOperationException($"Không tìm thấy sản phẩm với ID: {productId}");
                }

                _logger.LogInformation($"Successfully found product: {product.Name} (ID: {product.Id})");
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving product with ID {productId}: {ex.Message}");
                throw new InvalidOperationException($"Có lỗi xảy ra khi tìm kiếm sản phẩm: {ex.Message}");
            }
        }

        // Xoá sản phẩm khỏi giỏ hàng
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart is not null)
            {
                cart.RemoveItem(productId);
                // Lưu lại giỏ hàng vào Session sau khi đã xóa mục
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        // Xóa tất cả sản phẩm khỏi giỏ hàng
        public IActionResult RemoveAllFromCart()
        {
            // Lấy giỏ hàng từ session
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null)
            {
                // Xóa tất cả sản phẩm trong giỏ hàng
                cart.Items.Clear();
                // Cập nhật giỏ hàng vào session
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Checkout()
        {
            return View(new Order());
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order, List<int> selectedProductIds)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            var pendingOrder = HttpContext.Session.GetObjectFromJson<Order>("PendingOrder");

            if (cart == null || pendingOrder == null || !pendingOrder.OrderDetails.Any())
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra, vui lòng thử lại!";
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để thanh toán!";
                return RedirectToAction("Login");
            }

            // Cập nhật thông tin đơn hàng
            order.UserId = user.Id;
            order.OrderDate = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;
            order.TotalPrice = pendingOrder.TotalPrice;
            order.OrderDetails = pendingOrder.OrderDetails;
            

            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Có lỗi khi xử lý đơn hàng, vui lòng thử lại!";
                return RedirectToAction("Index");
            }

            // Xóa sản phẩm đã thanh toán khỏi giỏ hàng
            cart.Items = cart.Items.Where(i => !selectedProductIds.Contains(i.ProductId)).ToList();
            HttpContext.Session.SetObjectAsJson("Cart", cart);
           
            // Xóa đơn hàng tạm thời khỏi Session
            HttpContext.Session.Remove("PendingOrder");

            return RedirectToAction("OrderCompleted", new { id = order.Id });
        }


        [HttpPost]
        public IActionResult ProceedToCheckout(List<int> selectedProductIds)
        {
            if (selectedProductIds == null || !selectedProductIds.Any())
            {
                TempData["ErrorMessage"] = "Vui lòng chọn ít nhất một sản phẩm để thanh toán!";
                return RedirectToAction("Index"); // Quay lại giỏ hàng
            }

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || cart.Items == null || cart.Items.Count == 0)
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index");
            }

            var selectedItems = cart.Items.Where(i => selectedProductIds.Contains(i.ProductId)).ToList();
            if (!selectedItems.Any())
            {
                TempData["ErrorMessage"] = "Không có sản phẩm hợp lệ để thanh toán!";
                return RedirectToAction("Index");
            }

            // Tạo đơn hàng tạm thời để hiển thị trên trang Checkout
            var order = new Order
            {
                OrderDetails = selectedItems.Select(i => new OrderDetail
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Subtotal = i.Price * i.Quantity
                }).ToList(),
                TotalPrice = selectedItems.Sum(i => i.Price * i.Quantity),
                FullName = "",
                PhoneNumber = "",
            };

            // Lưu đơn hàng tạm thời vào Session
            HttpContext.Session.SetObjectAsJson("PendingOrder", order);
            HttpContext.Session.SetObjectAsJson("SelectedProductIds", selectedProductIds);

            Console.WriteLine("Selected Product IDs: " + string.Join(", ", selectedProductIds));

            return View("Checkout", order);
        }
        public async Task<IActionResult> OrderCompleted(int id)
        {
            var order = await _context.Orders
               .Include(o => o.OrderDetails)
                   .ThenInclude(od => od.Product)
               .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        [HttpPost]
        public JsonResult UpdateCart([FromBody] CartUpdateRequest request)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
                if (item != null)
                {
                    item.Quantity = request.Quantity;
                }

                // 🔥 Lưu giỏ hàng vào Session
                HttpContext.Session.SetObjectAsJson("Cart", cart);

                return Json(new { success = true, totalPrice = cart.Items.Sum(i => i.Price * i.Quantity) });
            }

            return Json(new { success = false, message = "Giỏ hàng không tồn tại" });
        }

        // 🛒 Tạo class để nhận dữ liệu từ AJAX
        public class CartUpdateRequest
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        public IActionResult OrderDetails(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product) // Nạp sản phẩm để tránh null
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


        public IActionResult OrderList()
        {
            var userId = _userManager.GetUserId(User); // Lấy ID của người dùng đang đăng nhập

            var orders = _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }
    }
}
