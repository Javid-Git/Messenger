using AllupNew.Models;
using AllupNew.ViewModels.BasketViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllupNew.ViewModels.OrderViewModels
{
    public class OrderVM
    {
        public Order Order { get; set; }

        public List<Basket> Baskets { get; set; }
    }
}
