using AllupNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllupNew.ViewModels.HomeViewModels
{
    public class HomeViewModel
    {
        public List<Product> Products { get; set; }
        public List<Slider> Sliders { get; set; }

        public List<Product> BestSeller { get; set; }
        public List<Product> Feature { get; set; }
        public List<Product> NewArrivel { get; set; }
    }
}
