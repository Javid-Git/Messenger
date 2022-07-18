using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AllupNew.DAL;
using AllupNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllupNew.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public SliderViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<Slider> sliders)
        {
            return View(await Task.FromResult(sliders));
        }
    }
}
