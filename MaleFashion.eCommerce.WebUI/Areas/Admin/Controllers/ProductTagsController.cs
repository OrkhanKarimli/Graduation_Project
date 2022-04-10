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
    public class ProductTagsController : Controller
    {
        private readonly FashionDbContext _context;

        public ProductTagsController(FashionDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductTags
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductTags.ToListAsync());
        }

        // GET: Admin/ProductTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTag = await _context.ProductTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productTag == null)
            {
                return NotFound();
            }

            return View(productTag);
        }

        // GET: Admin/ProductTags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ProductTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductTagName,Id,CreatedDate,UpdatedDate,DeletedDate")] ProductTag productTag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productTag);
        }

        // GET: Admin/ProductTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTag = await _context.ProductTags.FindAsync(id);
            if (productTag == null)
            {
                return NotFound();
            }
            return View(productTag);
        }

        // POST: Admin/ProductTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductTagName,Id,CreatedDate,UpdatedDate,DeletedDate")] ProductTag productTag)
        {
            if (id != productTag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    productTag.UpdatedDate = DateTime.UtcNow.AddHours(4);
                    _context.Update(productTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductTagExists(productTag.Id))
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
            return View(productTag);
        }

        // GET: Admin/ProductTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTag = await _context.ProductTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productTag == null)
            {
                return NotFound();
            }

            return View(productTag);
        }

        // POST: Admin/ProductTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productTag = await _context.ProductTags.FindAsync(id);
            _context.ProductTags.Remove(productTag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductTagExists(int id)
        {
            return _context.ProductTags.Any(e => e.Id == id);
        }
    }
}
