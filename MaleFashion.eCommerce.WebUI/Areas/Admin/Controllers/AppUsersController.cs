using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MaleFashion.eCommerce.WebUI.Models.DataContext;
using MaleFashion.eCommerce.WebUI.Models.Entity.Membership;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MaleFashion.eCommerce.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppUsersController : Controller
    {
        private readonly FashionDbContext _context;
        private readonly IWebHostEnvironment env;

        public AppUsersController(FashionDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        //// GET: Admin/AppUsers/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Admin/AppUsers/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Name,Surname,ImagePath,City,JobName,Age,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] AppUser appUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(appUser);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(appUser);
        //}

        // GET: Admin/AppUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.Users.FindAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }
            return View(appUser);
        }

        // POST: Admin/AppUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Surname,ImageTemp,City,JobName,Age,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] AppUser appUser, IFormFile image)
        {

            if (id != appUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string fullPath = null;
                string currentPath = null;

                AppUser entity = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

                if (entity.ImagePath != null || (entity.ImagePath == null && image != null))
                {
                    if (image == null && !string.IsNullOrWhiteSpace(appUser.ImageTemp))
                    {
                        appUser.ImagePath = entity.ImagePath;
                    }
                    else if (image == null)
                    {
                        currentPath = Path.Combine(env.WebRootPath, "admin-assets", "img", "users", entity.ImagePath);
                    }
                    else if (image != null)
                    {
                        string ext = Path.GetExtension(image.FileName);
                        string fileName = $"user-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                        fullPath = Path.Combine(env.WebRootPath, "admin-assets", "img", "users", fileName);

                        using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                        {
                            image.CopyTo(fs);
                        }

                        appUser.ImagePath = fileName;
                    }
                }

                try
                {
                    appUser.NormalizedUserName = appUser.UserName.ToUpper();
                    appUser.NormalizedEmail = appUser.Email.ToUpper();
                    _context.Update(appUser);
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

                    if (!AppUserExists(appUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), "PersonalSide");
            }
            return View(appUser);
        }

        private bool AppUserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
