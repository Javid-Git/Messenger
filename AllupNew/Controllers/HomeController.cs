using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using AllupNew.DAL;
using AllupNew.ViewModels.BasketViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllupNew.ViewModels.HomeViewModels;
using AllupNew.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace AllupNew.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products.ToListAsync();

            HomeViewModel homeViewModel = new HomeViewModel
            {
                Products = await _context.Products.ToListAsync(),
                Sliders = await _context.Sliders.ToListAsync(),
                BestSeller = products.Where(p=>p.IsBestSeller).ToList(),
                Feature = products.Where(p => p.IsFeature).ToList(),
                NewArrivel = products.Where(p => p.IsNewArrivel).ToList()
            };

            return View(homeViewModel);
        }

        [Authorize(Roles ="Member")]
        public async Task<IActionResult> Chat()
        {
            List<AppUser> users = await _userManager.Users.Where(u => !u.IsAdmin && u.UserName != User.Identity.Name).ToListAsync();

            return View(users);
        }

        //public  IActionResult GetSession()
        //{
        //    string session = HttpContext.Session.GetString("p129");

        //    return Content(session);
        //}

        //public IActionResult GetCookie()
        //{
        //    string session = HttpContext.Request.Cookies["p129"];

        //    return Content(session);
        //}
    }
}
