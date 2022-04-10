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
    public class MapsController : Controller
    {
        private readonly FashionDbContext _context;

        public MapsController(FashionDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Maps
        public async Task<IActionResult> Index()
        {
            return View(await _context.Maps.Where(m => m.DeletedDate == null).ToListAsync());
        }

        // GET: Admin/Maps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var map = await _context.Maps
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedDate == null);
            if (map == null)
            {
                return NotFound();
            }

            return View(map);
        }

        // GET: Admin/Maps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Maps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Info,Content,Id,CreatedDate,UpdatedDate,DeletedDate")] Map map)
        {
            if (ModelState.IsValid)
            {
                _context.Add(map);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(map);
        }

        // GET: Admin/Maps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var map = await _context.Maps.FirstOrDefaultAsync(m => m.Id == id && m.DeletedDate == null);
            if (map == null)
            {
                return NotFound();
            }
            return View(map);
        }

        // POST: Admin/Maps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Info,Content,Id,CreatedDate,UpdatedDate,DeletedDate")] Map map)
        {
            if (id != map.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(map);
                    map.UpdatedDate = DateTime.UtcNow.AddHours(4);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MapExists(map.Id))
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
            return View(map);
        }

        // GET: Admin/Maps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var map = await _context.Maps
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedDate == null);
            if (map == null)
            {
                return NotFound();
            }

            return View(map);
        }

        // POST: Admin/Maps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var map = await _context.Maps.FirstOrDefaultAsync(m => m.Id == id && m.DeletedDate == null);
            map.DeletedDate = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MapExists(int id)
        {
            return _context.Maps.Any(e => e.Id == id);
        }
    }
}
