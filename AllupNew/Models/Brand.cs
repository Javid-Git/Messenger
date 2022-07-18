using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AllupNew.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:255)]
        public string Name { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool IsUpdated { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }


        public IEnumerable<Product> Products { get; set; }
    }
}
