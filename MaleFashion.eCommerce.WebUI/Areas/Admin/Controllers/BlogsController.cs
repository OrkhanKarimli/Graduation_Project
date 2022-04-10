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
using MaleFashion.eCommerce.WebUI.AppCode.Extensions;
using MaleFashion.eCommerce.WebUI.Models.Entity.Membership;

namespace MaleFashion.eCommerce.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {
        private readonly FashionDbContext _context;
        private readonly IWebHostEnvironment env;

        public BlogsController(FashionDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: Admin/Blogs
        public async Task<IActionResult> Index()
        {
            var fashionDbContext = _context.Blogs.Include(b => b.Aphorism);
            return View(await fashionDbContext.ToListAsync());
        }

        [HttpPost]
        async public Task<IActionResult> Index(string searchText)
        {
            var query = _context.Blogs
                        .Include(a => a.Aphorism)
                        .AsQueryable();

            if (searchText != null && await _context.Blogs.AnyAsync(b => b.Title.Contains(searchText)))
            {
                query = query.Where(b => b.Title.Contains(searchText));
            }
            else if (searchText != null && await _context.Blogs.AnyAsync(b => b.Description.Contains(searchText)))
            {
                query = query.Where(b => b.Description.Contains(searchText));
            }
            else if (searchText != null && await _context.Blogs.AnyAsync(b => b.AuthorName.Contains(searchText)))
            {
                query = query.Where(b => b.AuthorName.Contains(searchText));
            }
            else if (searchText != null && await _context.Blogs.AnyAsync(b => b.AuthorSurname.Contains(searchText)))
            {
                query = query.Where(b => b.AuthorSurname.Contains(searchText));
            }
            else if (searchText != null && await _context.Blogs.AnyAsync(b => b.Aphorism.Author.Contains(searchText)))
            {
                query = query.Where(b => b.Aphorism.Author.Contains(searchText));
            }

            var data = query.ToList();

            if (Request.IsAjaxRequest())
            {
                return Json(data);
            }

            return View(data);
        }

        // GET: Admin/Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.Aphorism)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Admin/Blogs/Create
        public IActionResult Create()
        {
            ViewData["AphorismId"] = new SelectList(_context.Aphorisms, "Id", "Author");
            return View();
        }

        // POST: Admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,ImagePath,AphorismId,Id")] Blog blog, IFormFile image)
        {
            if (image == null)
            {
                ModelState.AddModelError("imageSelectError", "Şəkil seçməyibsiniz!");
            }
            else
            {
                var ext = Path.GetExtension(image.FileName);
                var fileName = $"blog-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                var fullPath = Path.Combine(env.WebRootPath, "assets", "images", "blog", fileName);

                using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    image.CopyTo(fs);
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        blog.ImagePath = fileName;
                    }
                    catch (Exception)
                    {
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }

                    AppUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == User.GetUserId());
                    blog.AuthorImagePath = user.ImagePath;
                    blog.AuthorName = user.Name;
                    blog.AuthorSurname = user.Surname;
                    _context.Add(blog);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["AphorismId"] = new SelectList(_context.Aphorisms, "Id", "Author", blog.AphorismId);
            }

            return View(blog);
        }

        // GET: Admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewData["AphorismId"] = new SelectList(_context.Aphorisms, "Id", "Author", blog.AphorismId);
            return View(blog);
        }

        // POST: Admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Description,ImageTemp,AphorismId,Id")] Blog blog, IFormFile image)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string fullpath = null;
                string currentpath = null;

                var entity = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);

                if (image == null && !string.IsNullOrWhiteSpace(blog.ImageTemp))
                {
                    blog.ImagePath = entity.ImagePath;
                }
                else if (image == null)
                {
                    currentpath = Path.Combine(Path.Combine(env.WebRootPath, "assets", "images", "blog", entity.ImagePath));
                }
                else if (image != null)
                {
                    var ext = Path.GetExtension(image.FileName);
                    var fileName = $"blog-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                    fullpath = Path.Combine(env.WebRootPath, "assets", "images", "blog", fileName);

                    using (var fs = new FileStream(fullpath, FileMode.Create, FileAccess.Write))
                    {
                        image.CopyTo(fs);
                    }

                    blog.ImagePath = fileName;
                }

                try
                {
                    _context.Update(blog);
                    blog.UpdatedDate = DateTime.UtcNow.AddHours(4);
                    await _context.SaveChangesAsync();

                    if (System.IO.File.Exists(currentpath) && !string.IsNullOrWhiteSpace(currentpath))
                    {
                        System.IO.File.Delete(currentpath);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {

                    if (System.IO.File.Exists(fullpath) && !string.IsNullOrWhiteSpace(fullpath))
                    {
                        System.IO.File.Delete(fullpath);
                    }

                    if (!BlogExists(blog.Id))
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
            ViewData["AphorismId"] = new SelectList(_context.Aphorisms, "Id", "Id", blog.AphorismId);
            return View(blog);
        }

        // GET: Admin/Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.Aphorism)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Admin/Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
    }
}
