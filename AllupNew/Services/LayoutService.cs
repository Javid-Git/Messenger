﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using AllupNew.DAL;
using AllupNew.Interfaces;
using AllupNew.Models;
using AllupNew.ViewModels.BasketViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllupNew.Services
{
    public class LayoutService : ILayoutService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LayoutService(IHttpContextAccessor httpContextAccessor, AppDbContext context, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<BasketVM>> GetBasket()
        {
            string basket = _httpContextAccessor.HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = null;

            if (!string.IsNullOrWhiteSpace(basket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            }
            else
            {
                basketVMs = new List<BasketVM>();
            }

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users.Include(u => u.Baskets).FirstOrDefaultAsync(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);

                if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                {
                    foreach (var item in appUser.Baskets)
                    {
                        if (!basketVMs.Any(b=>b.ProductId == item.ProductId))
                        {
                            BasketVM basketVM = new BasketVM
                            {
                                ProductId = item.ProductId,
                                Count = item.Count
                            };

                            basketVMs.Add(basketVM);
                        }
                    }

                    basket = JsonConvert.SerializeObject(basketVMs);

                    _httpContextAccessor.HttpContext.Response.Cookies.Append("basket", basket);
                }
            }

            foreach (BasketVM basketVM in basketVMs)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketVM.ProductId);

                basketVM.Name = product.Name;
                basketVM.Price = product.DiscoutnPrice > 0 ? product.DiscoutnPrice : product.Price;
                basketVM.Image = product.MainImage;
                basketVM.ExTax = product.ExTax;
            }

            return basketVMs;
        }

        public async Task<IDictionary<string, string>> GetSetting()
        {
            IDictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value);

            return settings;
        }
    }
}
