﻿using AllupNew.ViewModels.BasketViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllupNew.ViewModels.HeaderViewModels
{
    public class HeaderViewModel
    {
        public IDictionary<string,string> Settings { get; set; }
        public List<BasketVM> BasketVMs { get; set; }
    }
}
