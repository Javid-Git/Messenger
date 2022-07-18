using AllupNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllupNew.ViewModels.AccountViewModels
{
    public class MemberVM
    {
        public ProfileVM ProfileVM { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
