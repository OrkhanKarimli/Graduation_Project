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
using Microsoft.AspNetCore.Authorization;

namespace MaleFashion.eCommerce.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("index.appinfos")]
    public class AppInfosController : Controller
    {
        private readonly FashionDbContext _context;
        readonly IWebHostEnvironment env;

        public AppInfosController(FashionDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: Admin/AppInfos
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppInfos.ToListAsync());
        }

        // GET: Admin/AppInfos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appInfo = await _context.AppInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appInfo == null)
            {
                return NotFound();
            }

            return View(appInfo);
        }

        // GET: Admin/AppInfos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AppInfos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HeaderLogoPath,FooterLogoPath,Description,CardsLogoPath,ContactTitle,ContactDescription,FooterSiteInfo,Id")] AppInfo appInfo, IFormFile headerImg, IFormFile footerImg, IFormFile cardImg)
        {
            if (headerImg == null || footerImg == null || cardImg == null)
            {
                ModelState.AddModelError("imageSelectError", "Şəkil seçməyibsiniz!");
            }
            else
            {
                var ext = Path.GetExtension(headerImg.FileName);
                var fileName = $"header-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                var fullPath = Path.Combine(env.WebRootPath, "assets", "images", "index", fileName);

                using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    headerImg.CopyTo(fs);
                }

                var extf = Path.GetExtension(footerImg.FileName);
                var fileNamef = $"footer-{Guid.NewGuid().ToString().Replace("-", "")}{extf}";
                var fullPathf = Path.Combine(env.WebRootPath, "assets", "images", "index", fileNamef);

                using (var fs = new FileStream(fullPathf, FileMode.Create, FileAccess.Write))
                {
                    footerImg.CopyTo(fs);
                }

                var extc = Path.GetExtension(cardImg.FileName);
                var fileNamec = $"card-{Guid.NewGuid().ToString().Replace("-", "")}{extc}";
                var fullPathc = Path.Combine(env.WebRootPath, "assets", "images", "index", fileNamec);

                using (var fs = new FileStream(fullPathc, FileMode.Create, FileAccess.Write))
                {
                    cardImg.CopyTo(fs);
                }


                if (ModelState.IsValid)
                {
                    try
                    {
                        appInfo.HeaderLogoPath = fileName;
                        appInfo.FooterLogoPath = fileNamef;
                        appInfo.CardsLogoPath = fileNamec;
                    }
                    catch (Exception)
                    {
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }

                        if (System.IO.File.Exists(fullPathf))
                        {
                            System.IO.File.Delete(fullPathf);
                        }

                        if (System.IO.File.Exists(fullPathc))
                        {
                            System.IO.File.Delete(fullPathc);
                        }
                    }

                    _context.Add(appInfo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(appInfo);
        }

        // GET: Admin/AppInfos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appInfo = await _context.AppInfos.FindAsync(id);
            if (appInfo == null)
            {
                return NotFound();
            }
            return View(appInfo);
        }

        // POST: Admin/AppInfos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Description,ContactTitle,ContactDescription,FooterSiteInfo,Id, HeaderImgTemp, FooterImgTemp, CardImgTemp")] AppInfo appInfo, IFormFile headerImg, IFormFile footerImg, IFormFile cardImg)
        {
            if (id != appInfo.Id)
            {
                return NotFound();
            }

            var entity = _context.AppInfos.AsNoTracking().FirstOrDefault(i => i.Id == id);
            string fullPath = null;
            string currentPath = null;

            if (headerImg == null && !string.IsNullOrWhiteSpace(appInfo.HeaderLogoPath))
            {
                appInfo.HeaderImgTemp = entity.HeaderImgTemp;
            }
            else if (headerImg == null)
            {
                currentPath = Path.Combine(env.WebRootPath, "assets", "images", "index", entity.HeaderImgTemp);
            }
            else if (headerImg != null)
            {
                var ext = Path.GetExtension(headerImg.FileName);
                var fileName = $"header-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                fullPath = Path.Combine(env.WebRootPath, "uploads", "images", "index", fileName);

                using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    headerImg.CopyTo(fs);
                }

                appInfo.HeaderImgTemp = fileName;
            }

            string fullPathf = null;
            string currentPathf = null;

            if (footerImg == null && !string.IsNullOrWhiteSpace(appInfo.FooterLogoPath))
            {
                appInfo.FooterLogoPath = entity.FooterLogoPath;
            }
            else if (footerImg == null)
            {
                currentPathf = Path.Combine(env.WebRootPath, "assets", "images", "index", entity.FooterLogoPath);
            }
            else if (footerImg != null)
            {
                var ext = Path.GetExtension(footerImg.FileName);
                var fileName = $"footer-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                fullPathf = Path.Combine(env.WebRootPath, "uploads", "images", "index", fileName);

                using (var fs = new FileStream(fullPathf, FileMode.Create, FileAccess.Write))
                {
                    footerImg.CopyTo(fs);
                }

                appInfo.FooterLogoPath = fileName;
            }

            string fullPathc = null;
            string currentPathc = null;

            if (cardImg == null && !string.IsNullOrWhiteSpace(appInfo.CardsLogoPath))
            {
                appInfo.CardsLogoPath = entity.CardsLogoPath;
            }
            else if (cardImg == null)
            {
                currentPathc = Path.Combine(env.WebRootPath, "assets", "images", "index", entity.CardsLogoPath);
            }
            else if (headerImg != null)
            {
                var ext = Path.GetExtension(cardImg.FileName);
                var fileName = $"header-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                fullPathc = Path.Combine(env.WebRootPath, "uploads", "images", "index", fileName);

                using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    cardImg.CopyTo(fs);
                }

                appInfo.CardsLogoPath = fileName;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    appInfo.UpdatedDate = DateTime.UtcNow.AddHours(4);
                    _context.Update(appInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppInfoExists(appInfo.Id))
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
            return View(appInfo);
        }

        // GET: Admin/AppInfos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appInfo = await _context.AppInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appInfo == null)
            {
                return NotFound();
            }

            return View(appInfo);
        }

        // POST: Admin/AppInfos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appInfo = await _context.AppInfos.FindAsync(id);
            _context.AppInfos.Remove(appInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppInfoExists(int id)
        {
            return _context.AppInfos.Any(e => e.Id == id);
        }
    }
}
