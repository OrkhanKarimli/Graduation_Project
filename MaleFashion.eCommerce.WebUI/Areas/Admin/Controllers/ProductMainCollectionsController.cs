using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MaleFashion.eCommerce.WebUI.Models.DataContext;
using MaleFashion.eCommerce.WebUI.Models.Entity;

namespace MaleFashion.eCommerce.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductMainCollectionsController : Controller
    {
        private readonly FashionDbContext _context;

        public ProductMainCollectionsController(FashionDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductMainCollections
        public IActionResult Index()
        {
            IQueryable<ProductMainCollection> query = _context.ProductMainCollections
                                                              .Include(p => p.Category)
                                                              .Include(p => p.Color)
                                                              .Include(p => p.Product)
                                                              .Include(p => p.Product.ProductImages)
                                                              .Include(p => p.ProductTag)
                                                              .Include(p => p.Size)
                                                              .AsQueryable();

            IEnumerable<ProductMainCollection> data = query.ToList();

            return View(data);
        }

        // GET: Admin/ProductMainCollections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productMainCollection = await _context.ProductMainCollections
                                              .Include(p => p.Category)
                                              .Include(p => p.Color)
                                              .Include(p => p.Product)
                                              .Include(p => p.Product.ProductImages)
                                              .Include(p => p.ProductTag)
                                              .Include(p => p.Size)
                                              .FirstOrDefaultAsync(m => m.Id == id);

            if (productMainCollection == null)
            {
                return NotFound();
            }

            return View(productMainCollection);
        }

        // GET: Admin/ProductMainCollections/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorName");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Title");
            ViewData["ProductTagId"] = new SelectList(_context.ProductTags, "Id", "ProductTagName");
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "SizeName");
            return View();
        }

        // POST: Admin/ProductMainCollections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Price,CategoryId,ProductId,ColorId,ProductTagId,SizeId")] ProductMainCollection productMainCollection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productMainCollection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", productMainCollection.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "Id", productMainCollection.ColorId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", productMainCollection.ProductId);
            ViewData["ProductTagId"] = new SelectList(_context.ProductTags, "Id", "Id", productMainCollection.ProductTagId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Id", productMainCollection.SizeId);
            return View(productMainCollection);
        }

        // GET: Admin/ProductMainCollections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productMainCollection = await _context.ProductMainCollections.FindAsync(id);
            if (productMainCollection == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", productMainCollection.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorName", productMainCollection.ColorId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Title", productMainCollection.ProductId);
            ViewData["ProductTagId"] = new SelectList(_context.ProductTags, "Id", "ProductTagName", productMainCollection.ProductTagId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "SizeName", productMainCollection.SizeId);
            return View(productMainCollection);
        }

        // POST: Admin/ProductMainCollections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,CategoryId,ProductId,ColorId,ProductTagId,SizeId")] ProductMainCollection productMainCollection)
        {
            if (id != productMainCollection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productMainCollection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductMainCollectionExists(productMainCollection.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", productMainCollection.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "Id", productMainCollection.ColorId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", productMainCollection.ProductId);
            ViewData["ProductTagId"] = new SelectList(_context.ProductTags, "Id", "Id", productMainCollection.ProductTagId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Id", productMainCollection.SizeId);
            return View(productMainCollection);
        }

        // GET: Admin/ProductMainCollections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productMainCollection = await _context.ProductMainCollections
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Product)
                .Include(p => p.ProductTag)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productMainCollection == null)
            {
                return NotFound();
            }

            return View(productMainCollection);
        }

        // POST: Admin/ProductMainCollections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productMainCollection = await _context.ProductMainCollections.FindAsync(id);
            _context.ProductMainCollections.Remove(productMainCollection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductMainCollectionExists(int id)
        {
            return _context.ProductMainCollections.Any(e => e.Id == id);
        }
    }
}
