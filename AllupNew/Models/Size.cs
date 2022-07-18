using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AllupNew.Models
{
    public class Size
    {
        public int Id { get; set; }
        [StringLength(255), Required]
        public string Name { get; set; }
        public IEnumerable<ProductColorSize> ProductColorSizes { get; set; }
    }
}
