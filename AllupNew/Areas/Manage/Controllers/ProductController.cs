using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AllupNew.DAL;
using AllupNew.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AllupNew.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using AllupNew.Helpers;

namespace AllupNew.Areas.Manage.Controllers
{
    [Area("manage")]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    public class ProductController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(b => !b.IsDeleted && !b.IsMain).ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(b => !b.IsDeleted && !b.IsMain).ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();

            if (!ModelState.IsValid) return View();

            if (product.TagIds == null && product.Count <= 0)
            {
                ModelState.AddModelError("TagIds", "Tag Id s Is Required");
                return View();
            }

            if (!await _context.Brands.AnyAsync(b=>!b.IsDeleted && b.Id == product.BrandId))
            {
                ModelState.AddModelError("BrandId", "Select Correct Brand");
                return View();
            }

            if(product.CategoryId == null)
            {
                ModelState.AddModelError("CategoryId", "You Must Select Correct");
                return View();
            }

            if (!await _context.Categories.AnyAsync(c=>!c.IsDeleted && !c.IsMain && c.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Select Correct Category");
                return View();
            }

            if (product.ColorIds.Count() != product.SizeIds.Count() || product.ColorIds.Count() != product.Counts.Count() ||product.SizeIds.Count() != product.Counts.Count())
            {
                ModelState.AddModelError("", "Incorrect List");
                return View();
            }

            List<ProductColorSize> productColorSizes = new List<ProductColorSize>();

            for (int i = 0; i < product.ColorIds.Count(); i++)
            {
                if (!await _context.Colors.AnyAsync(c=>c.Id == product.ColorIds[i]))
                {
                    ModelState.AddModelError("", $"this color id {product.ColorIds[i]} is Incorrect");
                    return View();
                }

                if (!await _context.Sizes.AnyAsync(c => c.Id == product.SizeIds[i]))
                {
                    ModelState.AddModelError("", $"this size id {product.SizeIds[i]} is Incorrect");
                    return View();
                }

                if (product.Counts[i] < 0)
                {
                    ModelState.AddModelError("", "Count is Incorrect");
                    return View();
                }

                ProductColorSize productColorSize = new ProductColorSize
                {
                    ColorId = product.ColorIds[i],
                    SizeId = product.SizeIds[i],
                    Count = product.Counts[i]
                };

                productColorSizes.Add(productColorSize);
            }

            product.ProductColorSizes = productColorSizes;

            List<ProductTag> productTags = new List<ProductTag>();

            foreach (int tagId in product.TagIds)
            {
                if (!await _context.Tags.AnyAsync(t=>t.Id == tagId))
                {
                    ModelState.AddModelError("TagIds", $"Tag {tagId} IsCorrect");
                    return View();
                }

                ProductTag productTag = new ProductTag
                {
                    TagId = tagId
                };

                productTags.Add(productTag);
            }

            product.ProductTags = productTags;

            if (product.Files != null && product.Files.Count() > 5)
            {
                ModelState.AddModelError("Files", "Can Select Maximum 5 Image");
                return View();
            }

            if (product.MainFile != null)
            {
                if (!product.MainFile.CheckContentType("image/jpeg"))
                {
                    ModelState.AddModelError("MainFile", "Main Image Must Be .jpg");
                    return View();
                }

                if (product.MainFile.CheckFileLength(50))
                {
                    ModelState.AddModelError("MainFile", "Main Image Length Must Be 50kb");
                    return View();
                }

                product.MainImage = await product.MainFile.CreateAsync(_env, "assets", "images", "product");
            }
            else
            {
                ModelState.AddModelError("MainFile", "Main Image Is Requered");
                return View();
            }

            if (product.HoveFile != null)
            {
                if (!product.HoveFile.CheckContentType("image/jpeg"))
                {
                    ModelState.AddModelError("MainFHoveFileile", "Second Image Must Be .jpg");
                    return View();
                }

                if (product.HoveFile.CheckFileLength(50))
                {
                    ModelState.AddModelError("HoveFile", "Second Image Length Must Be 50kb");
                    return View();
                }

                product.HoverImage = await product.HoveFile.CreateAsync(_env, "assets", "images", "product");
            }
            else
            {
                ModelState.AddModelError("HoveFile", "Second Image Is Requered");
                return View();
            }

            if (product.Files != null && product.Files.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();

                foreach (IFormFile file in product.Files)
                {
                    if (!file.CheckContentType("image/jpeg"))
                    {
                        ModelState.AddModelError("Files", "Files Image Must Be .jpg");
                        return View();
                    }

                    if (file.CheckFileLength(50))
                    {
                        ModelState.AddModelError("Files", "Files Image Length Must Be 50kb");
                        return View();
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = await file.CreateAsync(_env, "assets", "images", "product-quick")
                    };

                    productImages.Add(productImage);
                }

                product.ProductImages = productImages;
            }

            string seria = (_context.Brands.FirstOrDefault(b => b.Id == product.BrandId).Name.Substring(0, 2) + product.Name.Trim().Substring(0, 2)).ToLower();

            int code = _context.Products.OrderByDescending(p => p.Id).FirstOrDefault(p => p.Seria == seria) != null ? _context.Products.OrderByDescending(p => p.Id).FirstOrDefault(p => p.Seria == seria).Code += 1 : 1;

            product.Code = code;
            product.Seria = seria;
            product.Name = product.Name.Trim();
            product.Count = productColorSizes.Sum(x => x.Count);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products
                .Include(p=>p.ProductImages)
                .Include(p=>p.ProductTags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            product.TagIds = product.ProductTags.Select(x => x.TagId);

            ViewBag.Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(b => !b.IsDeleted && !b.IsMain).ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Product product)
        {
            ViewBag.Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(b => !b.IsDeleted && !b.IsMain).ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();

            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            Product dbProduct = await _context.Products
                .Include(p=>p.ProductImages)
                .Include(p=>p.ProductTags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (dbProduct == null) return NotFound();

            if (!await _context.Brands.AnyAsync(b => !b.IsDeleted && b.Id == product.BrandId))
            {
                ModelState.AddModelError("BrandId", "Select Correct Brand");
                return View();
            }

            if (product.CategoryId == null)
            {
                ModelState.AddModelError("CategoryId", "You Must Select Correct");
                return View();
            }

            if (!await _context.Categories.AnyAsync(c => !c.IsDeleted && !c.IsMain && c.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Select Correct Category");
                return View();
            }

            if (product.TagIds == null && product.Count <= 0)
            {
                ModelState.AddModelError("TagIds", "Tag Id s Is Required");
                return View();
            }

            int canSelecteCount = 5 - dbProduct.ProductImages.Count();

            if (product.Files != null &&  canSelecteCount < product.Files.Count())
            {
                ModelState.AddModelError("Files", $"Can Select {canSelecteCount} items");
                return View();
            }

            List<ProductTag> productTags = new List<ProductTag>();

            foreach (int tagId in product.TagIds)
            {
                if (!await _context.Tags.AnyAsync(t => t.Id == tagId))
                {
                    ModelState.AddModelError("TagIds", $"Tag {tagId} IsCorrect");
                    return View();
                }

                ProductTag productTag = new ProductTag
                {
                    TagId = tagId
                };

                productTags.Add(productTag);
            }
            _context.ProductTags.RemoveRange(dbProduct.ProductTags);

            dbProduct.ProductTags = productTags;

            if (product.MainFile != null)
            {
                if (!product.MainFile.CheckContentType("image/jpeg"))
                {
                    ModelState.AddModelError("MainFile", "Main Image Must Be .jpg");
                    return View();
                }

                if (product.MainFile.CheckFileLength(50))
                {
                    ModelState.AddModelError("MainFile", "Main Image Length Must Be 50kb");
                    return View();
                }

                FileHelper.DeleteFile(_env, dbProduct.MainImage, "assets", "images", "product");

                dbProduct.MainImage = await product.MainFile.CreateAsync(_env, "assets", "images", "product");
            }

            if (product.HoveFile != null)
            {
                if (!product.HoveFile.CheckContentType("image/jpeg"))
                {
                    ModelState.AddModelError("MainFHoveFileile", "Second Image Must Be .jpg");
                    return View();
                }

                if (product.HoveFile.CheckFileLength(50))
                {
                    ModelState.AddModelError("HoveFile", "Second Image Length Must Be 50kb");
                    return View();
                }

                FileHelper.DeleteFile(_env, dbProduct.HoverImage, "assets", "images", "product");

                dbProduct.HoverImage = await product.HoveFile.CreateAsync(_env, "assets", "images", "product");
            }

            if (product.Files != null && product.Files.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();

                foreach (IFormFile file in product.Files)
                {
                    if (!file.CheckContentType("image/jpeg"))
                    {
                        ModelState.AddModelError("Files", "Files Image Must Be .jpg");
                        return View();
                    }

                    if (file.CheckFileLength(50))
                    {
                        ModelState.AddModelError("Files", "Files Image Length Must Be 50kb");
                        return View();
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = await file.CreateAsync(_env, "assets", "images", "product-quick")
                    };

                    productImages.Add(productImage);
                }

                dbProduct.ProductImages.AddRange(productImages);
            }

            product.Name = product.Name.Trim();
            
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null) return BadRequest();

            ProductImage productImage = await _context.ProductImages.FirstOrDefaultAsync(p => p.Id == id);

            if (productImage == null) return NotFound();

            Product product = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == productImage.ProductId);

            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();

            FileHelper.DeleteFile(_env, productImage.Image, "assets", "images", "product-quick");

            return PartialView("_ProductImagePartial",product.ProductImages);
        }

        public async Task<IActionResult> GetProductColorSizePartial()
        {
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();

            return PartialView("_ProductColorSizePatial");
        }

        //public async Task<IActionResult> Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(Student student)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }

        //    return Content($"{student.Name} {student.SurName} {student.Age} {student.Email}");
        //}
    }

    //public class Student
    //{
    //    [Required(ErrorMessage ="Mecburidi qaqa")]
    //    [StringLength(maximumLength:255,ErrorMessage = "Maksimum 255 Simvol Ola Biler")]
    //    public string Name { get; set; }
    //    public string SurName { get; set; }
    //    [Display(Name ="Yas")]
    //    public int Age { get; set; }
    //    [EmailAddress]
    //    public string Email { get; set; }
    //}
}
