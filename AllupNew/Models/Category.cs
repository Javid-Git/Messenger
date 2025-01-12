﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllupNew.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Image { get; set; }
        public bool IsMain { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool IsUpdated { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> ParentId { get; set; }

        public Category Parent { get; set; }
        public IEnumerable<Category> Children { get; set; }
        public IEnumerable<Product> Products { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
}
