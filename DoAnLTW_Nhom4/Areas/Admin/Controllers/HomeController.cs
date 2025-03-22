using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DoAnLTW_Nhom4.Models;
using DoAnLTW_Nhom4.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DoAnLTW_Nhom4.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DoAnLTW_Nhom4.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Employee}")]
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IProductRepository productRepository, IOrderRepository orderRepository, UserManager<ApplicationUser> userManager)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            ViewBag.TotalProducts = products.Count();
            ViewBag.TotalOrders = (await _orderRepository.GetAllAsync()).Count();
            ViewBag.TotalUsers = (await _userManager.Users.ToListAsync()).Count();
            return View(products);
        }
    }
}
