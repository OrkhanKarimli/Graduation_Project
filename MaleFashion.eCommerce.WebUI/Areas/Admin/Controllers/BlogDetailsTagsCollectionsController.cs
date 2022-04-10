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
    public class BlogDetailsTagsCollectionsController : Controller
    {
        private readonly FashionDbContext _context;

        public BlogDetailsTagsCollectionsController(FashionDbContext context)
        {
            _context = context;
        }

        // GET: Admin/BlogDetailsTagsCollections
        public async Task<IActionResult> Index()
        {
            var fashionDbContext = _context.BlogDetailsTagsCollections.Include(b => b.Blog).Include(b => b.Tag);
            return View(await fashionDbContext.ToListAsync());
        }

        // GET: Admin/BlogDetailsTagsCollections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogDetailsTagsCollection = await _context.BlogDetailsTagsCollections
                .Include(b => b.Blog)
                .Include(b => b.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogDetailsTagsCollection == null)
            {
                return NotFound();
            }

            return View(blogDetailsTagsCollection);
        }

        // GET: Admin/BlogDetailsTagsCollections/Create
        public IActionResult Create()
        {
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Title");
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "TagName");
            return View();
        }

        // POST: Admin/BlogDetailsTagsCollections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BlogId,TagId")] BlogDetailsTagsCollection blogDetailsTagsCollection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blogDetailsTagsCollection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Id", blogDetailsTagsCollection.BlogId);
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Id", blogDetailsTagsCollection.TagId);
            return View(blogDetailsTagsCollection);
        }

        // GET: Admin/BlogDetailsTagsCollections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogDetailsTagsCollection = await _context.BlogDetailsTagsCollections.FindAsync(id);
            if (blogDetailsTagsCollection == null)
            {
                return NotFound();
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Title", blogDetailsTagsCollection.BlogId);
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "TagName", blogDetailsTagsCollection.TagId);
            return View(blogDetailsTagsCollection);
        }

        // POST: Admin/BlogDetailsTagsCollections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,TagId")] BlogDetailsTagsCollection blogDetailsTagsCollection)
        {
            if (id != blogDetailsTagsCollection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogDetailsTagsCollection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogDetailsTagsCollectionExists(blogDetailsTagsCollection.Id))
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
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Id", blogDetailsTagsCollection.BlogId);
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Id", blogDetailsTagsCollection.TagId);
            return View(blogDetailsTagsCollection);
        }

        // GET: Admin/BlogDetailsTagsCollections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogDetailsTagsCollection = await _context.BlogDetailsTagsCollections
                .Include(b => b.Blog)
                .Include(b => b.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogDetailsTagsCollection == null)
            {
                return NotFound();
            }

            return View(blogDetailsTagsCollection);
        }

        // POST: Admin/BlogDetailsTagsCollections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogDetailsTagsCollection = await _context.BlogDetailsTagsCollections.FindAsync(id);
            _context.BlogDetailsTagsCollections.Remove(blogDetailsTagsCollection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogDetailsTagsCollectionExists(int id)
        {
            return _context.BlogDetailsTagsCollections.Any(e => e.Id == id);
        }
    }
}
