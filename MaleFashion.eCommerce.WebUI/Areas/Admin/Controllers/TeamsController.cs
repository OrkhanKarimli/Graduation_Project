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
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MaleFashion.eCommerce.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamsController : Controller
    {
        private readonly FashionDbContext _context;
        private readonly IWebHostEnvironment env;

        public TeamsController(FashionDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: Admin/Teams
        public async Task<IActionResult> Index()
        {
            var fashionDbContext = _context.Teams.Include(t => t.TeamJob);
            return View(await fashionDbContext.ToListAsync());
        }

        // GET: Admin/Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.TeamJob)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: Admin/Teams/Create
        public IActionResult Create()
        {
            ViewData["TeamJobId"] = new SelectList(_context.TeamJobs, "Id", "JobName");
            return View();
        }

        // POST: Admin/Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImagePath,Name,Surname,TeamJobId,Id,CreatedDate,UpdatedDate,DeletedDate")] Team team, IFormFile file)
        {
            if (file == null)
            {
                ModelState.AddModelError("imageSelectError", "Şəkil seçməyibsiniz!");
            }
            else
            {
                string ext = Path.GetExtension(file.FileName);
                string fileName = $"team-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                string fullPath = Path.Combine(env.WebRootPath, "assets", "images", "about-us", fileName);

                using (FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fs);
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        team.ImagePath = fileName;
                    }
                    catch (Exception)
                    {
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }

                    _context.Add(team);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["TeamJobId"] = new SelectList(_context.TeamJobs, "Id", "Id", team.TeamJobId);
            }

            return View(team);
        }

        // GET: Admin/Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            ViewData["TeamJobId"] = new SelectList(_context.TeamJobs, "Id", "JobName", team.TeamJobId);
            return View(team);
        }

        // POST: Admin/Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FileTemp,Name,Surname,TeamJobId,Id,CreatedDate,UpdatedDate,DeletedDate")] Team team, IFormFile file)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var entity = _context.Teams.AsNoTracking().FirstOrDefault(i => i.Id == id);
                string fullPath = null;
                string currentPath = null;

                if (file == null && !string.IsNullOrWhiteSpace(team.FileTemp))
                {
                    team.ImagePath = entity.ImagePath;
                }
                else if (file == null)
                {
                    currentPath = Path.Combine(env.WebRootPath, "uploads", "assets", "images", "about-us", entity.ImagePath);
                }
                else if (file != null)
                {
                    string ext = Path.GetExtension(file.FileName);
                    string fileName = $"team-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                    fullPath = Path.Combine(env.WebRootPath, "assets", "images", "about-us", fileName);

                    using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                    {
                        file.CopyTo(fs);
                    }

                    team.ImagePath = fileName;
                }

                try
                {
                    team.UpdatedDate = DateTime.UtcNow.AddHours(4);
                    _context.Update(team);
                    await _context.SaveChangesAsync();

                    if (System.IO.File.Exists(currentPath) && !string.IsNullOrWhiteSpace(currentPath))
                    {
                        System.IO.File.Delete(currentPath);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (System.IO.File.Exists(fullPath) && !string.IsNullOrWhiteSpace(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    if (!TeamExists(team.Id))
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
            ViewData["TeamJobId"] = new SelectList(_context.TeamJobs, "Id", "Id", team.TeamJobId);
            return View(team);
        }

        // GET: Admin/Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.TeamJob)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Admin/Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}
