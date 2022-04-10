using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MaleFashion.eCommerce.WebUI.Models.DataContext;
using MaleFashion.eCommerce.WebUI.Models.Entity;
using Microsoft.AspNetCore.Hosting;
using MaleFashion.eCommerce.WebUI.Models.FormModel;
using System.IO;

namespace MaleFashion.eCommerce.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly FashionDbContext _context;
        private readonly IWebHostEnvironment env;

        public ProductsController(FashionDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var fashionDbContext = _context.Products.Include(p => p.ProductImages).Include(p => p.Brand);
            return View(await fashionDbContext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "BrandName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Files,BrandId,Id,CreatedDate,UpdatedDate,DeletedDate")] Product product)
        {
            product.ProductImages = new List<ProductImage>();

            foreach (ImageItemFormModel item in product.Files)
            {
                string ext = Path.GetExtension(item.File.FileName);
                string filename = $"product-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                string fullpath = Path.Combine(env.WebRootPath, "assets", "images", "shop", filename);

                using (FileStream fs = new FileStream(fullpath, FileMode.Create, FileAccess.Write))
                {
                    item.File.CopyTo(fs);
                }

                product.ProductImages.Add(new ProductImage
                {
                    IsMain = item.IsMain,
                    ImagePath = filename
                });
            }

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id", product.BrandId);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "BrandName", product.BrandId);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Description,Files,BrandId,Id,CreatedDate,UpdatedDate,DeletedDate")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                IEnumerable<Product> products = _context.Products.AsNoTracking().Where(p => p.Id == id);

                if (product == null)
                {
                    return NotFound();
                }

                IEnumerable<ProductImage> images = await _context.ProductImages.Where(pi => pi.ProductId == id).ToListAsync();

                foreach (ProductImage item in images)
                {
                    if (product.Files.Any(f => f.File == null && string.IsNullOrWhiteSpace(f.TempPath) && f.Id == item.Id))
                    {
                        _context.ProductImages.Remove(item);

                        string currentpath = Path.Combine(env.WebRootPath, "assets", "images", "shop", item.ImagePath);

                        if (System.IO.File.Exists(currentpath))
                        {
                            System.IO.File.Delete(currentpath);
                        }
                    }
                    else if (product.Files.Any(f => f.IsMain && f.Id == item.Id))
                    {
                        item.IsMain = true;
                    }
                    else
                    {
                        item.IsMain = false;
                    }
                }

                foreach (ImageItemFormModel item in product.Files.Where(f => f.File != null))
                {
                    string ext = Path.GetExtension(item.File.FileName);
                    string filename = $"product-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                    string fullpath = Path.Combine(env.WebRootPath, "assets", "images", "shop", filename);

                    using (FileStream fs = new FileStream(fullpath, FileMode.Create, FileAccess.Write))
                    {
                        item.File.CopyTo(fs);
                    }

                    product.ProductImages.Add(new ProductImage
                    {
                        IsMain = item.IsMain,
                        ImagePath = filename
                    });
                }

                try
                {
                    product.UpdatedDate = DateTime.UtcNow.AddHours(4);
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id", product.BrandId);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
