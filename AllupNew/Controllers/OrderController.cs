using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using AllupNew.DAL;
using AllupNew.Enums;
using AllupNew.Models;
using AllupNew.ViewModels.BasketViewModels;
using AllupNew.ViewModels.OrderViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllupNew.Controllers
{
    [Authorize(Roles ="Member")]
    public class OrderController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public OrderController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

            List<Basket> baskets = await _context.Baskets.Include(b=>b.Product).Where(b => b.AppUserId == appUser.Id).ToListAsync();

            Order order = new Order 
            {
                Name = appUser.Name,
                SurName = appUser.SurName,
                Email = appUser.Email
            };

            OrderVM orderVM = new OrderVM
            {
                Order = order,
                Baskets = baskets
            };

            return View(orderVM);
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(Order order)
        {
            AppUser appUser = await _userManager.Users.Include(u=>u.Baskets).ThenInclude(b=>b.Product).FirstOrDefaultAsync(u=>u.UserName == User.Identity.Name);

            List<OrderItem> orderItems = new List<OrderItem>();

            foreach (Basket basket in appUser.Baskets)
            {
                OrderItem orderItem = new OrderItem
                {
                    Price = basket.Product.DiscoutnPrice > 0 ? basket.Product.DiscoutnPrice : basket.Product.Price,
                    Count = basket.Count,
                    ProductId = basket.ProductId,
                    TotalPrice = basket.Product.DiscoutnPrice > 0 ? basket.Product.DiscoutnPrice * basket.Count : basket.Product.Price * basket.Count
                };

                orderItems.Add(orderItem);
            }

            order.OrderItems = orderItems;
            order.CreatedAt = DateTime.UtcNow.AddHours(4);
            order.AppUserId = appUser.Id;
            order.OrderStatus = OrderStatus.Pending;
            order.TotalPrice = orderItems.Sum(o => o.TotalPrice);

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("index", "home");
        }
    }
}
