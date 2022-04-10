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
    public class AphorismsController : Controller
    {
        private readonly FashionDbContext _context;

        public AphorismsController(FashionDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Aphorisms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Aphorisms.ToListAsync());
        }

        // GET: Admin/Aphorisms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aphorism = await _context.Aphorisms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aphorism == null)
            {
                return NotFound();
            }

            return View(aphorism);
        }

        [HttpPost]
        public IActionResult Index(string author, string content)
        {
            var query = _context.Aphorisms.AsQueryable();

            if (author != null)
            {
                query = query.Where(a => a.Author.Contains(author));
            }
            else if (content != null)
            {
                query = query.Where(a => a.Content.Contains(content));
            }

            IEnumerable<Aphorism> data = query.ToList();

            return View(data);
        }

        // GET: Admin/Aphorisms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Aphorisms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content,Author,Id,CreatedDate,UpdatedDate,DeletedDate")] Aphorism aphorism)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aphorism);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aphorism);
        }

        // GET: Admin/Aphorisms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aphorism = await _context.Aphorisms.FindAsync(id);
            if (aphorism == null)
            {
                return NotFound();
            }
            return View(aphorism);
        }

        // POST: Admin/Aphorisms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Content,Author,Id,CreatedDate,UpdatedDate,DeletedDate")] Aphorism aphorism)
        {
            if (id != aphorism.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aphorism);
                    aphorism.UpdatedDate = DateTime.UtcNow.AddHours(4);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AphorismExists(aphorism.Id))
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
            return View(aphorism);
        }

        // GET: Admin/Aphorisms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aphorism = await _context.Aphorisms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aphorism == null)
            {
                return NotFound();
            }

            return View(aphorism);
        }

        // POST: Admin/Aphorisms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aphorism = await _context.Aphorisms.FindAsync(id);
            _context.Aphorisms.Remove(aphorism);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AphorismExists(int id)
        {
            return _context.Aphorisms.Any(e => e.Id == id);
        }
    }
}
