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
    public class TeamJobsController : Controller
    {
        private readonly FashionDbContext _context;

        public TeamJobsController(FashionDbContext context)
        {
            _context = context;
        }

        // GET: Admin/TeamJobs
        public async Task<IActionResult> Index()
        {
            return View(await _context.TeamJobs.ToListAsync());
        }

        // GET: Admin/TeamJobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamJob = await _context.TeamJobs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teamJob == null)
            {
                return NotFound();
            }

            return View(teamJob);
        }

        // GET: Admin/TeamJobs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/TeamJobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobName,Id,CreatedDate,UpdatedDate,DeletedDate")] TeamJob teamJob)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teamJob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teamJob);
        }

        // GET: Admin/TeamJobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamJob = await _context.TeamJobs.FindAsync(id);
            if (teamJob == null)
            {
                return NotFound();
            }
            return View(teamJob);
        }

        // POST: Admin/TeamJobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JobName,Id,CreatedDate,UpdatedDate,DeletedDate")] TeamJob teamJob)
        {
            if (id != teamJob.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    teamJob.UpdatedDate = DateTime.UtcNow.AddHours(4);
                    _context.Update(teamJob);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamJobExists(teamJob.Id))
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
            return View(teamJob);
        }

        // GET: Admin/TeamJobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamJob = await _context.TeamJobs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teamJob == null)
            {
                return NotFound();
            }

            return View(teamJob);
        }

        // POST: Admin/TeamJobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamJob = await _context.TeamJobs.FindAsync(id);
            _context.TeamJobs.Remove(teamJob);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamJobExists(int id)
        {
            return _context.TeamJobs.Any(e => e.Id == id);
        }
    }
}
