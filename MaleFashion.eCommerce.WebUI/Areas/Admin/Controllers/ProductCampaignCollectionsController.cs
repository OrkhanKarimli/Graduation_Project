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
    public class ProductCampaignCollectionsController : Controller
    {
        private readonly FashionDbContext _context;

        public ProductCampaignCollectionsController(FashionDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductCampaignCollections
        public async Task<IActionResult> Index()
        {
            IEnumerable<ProductCampaignCollection> data = await _context.ProductCampaignCollections
                                                                .Include(p => p.Campaign)
                                                                .Include(p => p.ProductCollection)
                                                                .Include(p => p.ProductCollection.Product)
                                                                .ToListAsync();

            return View(data);
        }

        // GET: Admin/ProductCampaignCollections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCampaignCollection = await _context.ProductCampaignCollections
                                                  .Include(p => p.Campaign)
                                                  .Include(p => p.ProductCollection)
                                                  .Include(p => p.ProductCollection.Product)
                                                  .FirstOrDefaultAsync(m => m.Id == id);

            if (productCampaignCollection == null)
            {
                return NotFound();
            }

            return View(productCampaignCollection);
        }

        // GET: Admin/ProductCampaignCollections/Create
        public IActionResult Create()
        {
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "Id", "Title");
            ViewData["ProductCollectionId"] = new SelectList(_context.ProductMainCollections, "Id", "ProductId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductCollectionId,CampaignId")] ProductCampaignCollection productCampaignCollection)
        {
            ProductMainCollection productMain = await _context.ProductMainCollections.FirstOrDefaultAsync(p => p.Id == productCampaignCollection.ProductCollectionId);
            Campaign campaign = await _context.Campaigns.FindAsync(productCampaignCollection.CampaignId);

            decimal discount = productMain.Price * campaign.Discount / 100;

            productCampaignCollection.PriceNew = Math.Round(productMain.Price - discount, 2);

            if (ModelState.IsValid)
            {
                _context.Add(productCampaignCollection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "Id", "Title", productCampaignCollection.CampaignId);
            ViewData["ProductCollectionId"] = new SelectList(_context.ProductMainCollections, "Id", "ProductId", productCampaignCollection.ProductCollectionId);
            return View(productCampaignCollection);
        }

        // GET: Admin/ProductCampaignCollections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCampaignCollection = await _context.ProductCampaignCollections.FindAsync(id);
            if (productCampaignCollection == null)
            {
                return NotFound();
            }
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "Id", "Title", productCampaignCollection.CampaignId);
            ViewData["ProductCollectionId"] = new SelectList(_context.ProductMainCollections, "Id", "ProductId", productCampaignCollection.ProductCollectionId);
            return View(productCampaignCollection);
        }

        // POST: Admin/ProductCampaignCollections/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductCollectionId,CampaignId")] ProductCampaignCollection productCampaignCollection)
        {
            if (id != productCampaignCollection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ProductMainCollection productMain = await _context.ProductMainCollections.FirstOrDefaultAsync(p => p.Id == productCampaignCollection.ProductCollectionId);
                Campaign campaign = await _context.Campaigns.FindAsync(productCampaignCollection.CampaignId);

                decimal discount = productMain.Price * campaign.Discount / 100;

                productCampaignCollection.PriceNew = Math.Round(productMain.Price - discount, 2);

                try
                {
                    _context.Update(productCampaignCollection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCampaignCollectionExists(productCampaignCollection.Id))
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
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "Id", "Title", productCampaignCollection.CampaignId);
            ViewData["ProductCollectionId"] = new SelectList(_context.ProductMainCollections, "Id", "ProductId", productCampaignCollection.ProductCollectionId);
            return View(productCampaignCollection);
        }

        // GET: Admin/ProductCampaignCollections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCampaignCollection = await _context.ProductCampaignCollections
                .Include(p => p.Campaign)
                .Include(p => p.ProductCollection)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productCampaignCollection == null)
            {
                return NotFound();
            }

            return View(productCampaignCollection);
        }

        // POST: Admin/ProductCampaignCollections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productCampaignCollection = await _context.ProductCampaignCollections.FindAsync(id);
            _context.ProductCampaignCollections.Remove(productCampaignCollection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCampaignCollectionExists(int id)
        {
            return _context.ProductCampaignCollections.Any(e => e.Id == id);
        }
    }
}
