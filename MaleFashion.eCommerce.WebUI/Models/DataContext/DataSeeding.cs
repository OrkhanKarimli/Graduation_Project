using MaleFashion.eCommerce.WebUI.Models.Entity;
using MaleFashion.eCommerce.WebUI.Models.Entity.Membership;
using MaleFashion.eCommerce.WebUI.Models.Entity.Membership.Credentials;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.DataContext
{
    public static class DataSeeding
    {
        public static IApplicationBuilder DataSeed(this IApplicationBuilder app)
        {
            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                FashionDbContext db = scope.ServiceProvider.GetRequiredService<FashionDbContext>();
                RoleManager<AppRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
                UserManager<AppUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                // Auto Migrate (Update) Database
                //db.Database.Migrate();

                AppRole roleResult = roleManager.FindByNameAsync("Admin").Result;

                //IDENTITY DB AUTO SEED
                if (roleResult == null)
                {
                    roleResult = new AppRole
                    {
                        Name = "Admin"
                    };

                    IdentityResult roleResponse = roleManager.CreateAsync(roleResult).Result;

                    if (roleResponse.Succeeded)
                    {
                        AppUser userResult = userManager.FindByNameAsync("orkhankarimli").Result;

                        if (userResult == null)
                        {
                            userResult = new AppUser
                            {
                                UserName = "orkhankarimli",
                                Email = "orkhansk@code.edu.az"
                            };

                            IdentityResult userResponse = userManager.CreateAsync(userResult, AdminCredential.Pick()).Result;

                            if (userResponse.Succeeded)
                            {
                                var roleUserResult = userManager.AddToRoleAsync(userResult, roleResult.Name).Result;
                            }
                        }
                        else
                        {
                            var roleUserResult = userManager.AddToRoleAsync(userResult, roleResult.Name).Result;
                        }
                    }
                }
                else
                {
                    AppUser userResult = userManager.FindByNameAsync("orkhankarimli").Result;

                    if (userResult == null)
                    {
                        userResult = new AppUser
                        {
                            UserName = "orkhankarimli",
                            Email = "orkhansk@code.edu.az"
                        };

                        IdentityResult userResponse = userManager.CreateAsync(userResult, AdminCredential.Pick()).Result;

                        if (userResponse.Succeeded)
                        {
                            var roleUserResult = userManager.AddToRoleAsync(userResult, roleResult.Name).Result;
                        }
                    }
                    else
                    {
                        var roleUserResult = userManager.AddToRoleAsync(userResult, roleResult.Name).Result;
                    }
                }
                //IDENTITY DB AUTO SEED

                //AUTO CLAIM ADDING
                if (!db.UserClaims.Any())
                {
                    db.UserClaims.Add(new AppUserClaim
                    {
                        UserId = 1,
                        ClaimType = "admin.getprincipal",
                        ClaimValue = "1"
                    });

                    db.UserClaims.Add(new AppUserClaim
                    {
                        UserId = 1,
                        ClaimType = "admin.setprincipal",
                        ClaimValue = "1"
                    });

                    db.UserClaims.Add(new AppUserClaim
                    {
                        UserId = 1,
                        ClaimType = "index.appinfos",
                        ClaimValue = "1"
                    });

                    db.SaveChanges();
                }
                //AUTO CLAIM ADDING

                if (!db.AppInfos.Any())
                {
                    db.AppInfos.Add(new AppInfo
                    {
                        HeaderLogoPath = "download.webp",
                        FooterLogoPath = "download (1).webp",
                        Description = " The customer is at the heart of our unique business model, which includes design.",
                        CardsLogoPath = "xpayment.png.pagespeed.ic.baCZTAO1zx.webp",
                        ContactTitle = "NEWLETTER",
                        ContactDescription = " Be the first to know about new arrivals, look books, sales & promos!",
                        FooterSiteInfo = @"<p>
                Copyright ©
                <script>
                  document.write(new Date().getFullYear());
                </script>
                2021 2020 All rights reserved | This template is made with
                <i class='fa fa-heart-o' aria-hidden='true'></i> 
                by
                <a href = 'https://colorlib.com/' target = '_blank'>Colorlib</a>
              </p>"
                    });

                    db.SaveChanges();
                }

                if (!db.Aphorisms.Any())
                {
                    db.Aphorisms.Add(new Aphorism
                    {
                        Content = @"“When designing an advertisement for a particular product many
                                  things should be researched like where it should be
                                  displayed.”",
                        Author = "John Smith"
                    });

                    db.Aphorisms.Add(new Aphorism
                    {
                        Content = @"“It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, 
                                  to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, 
                                  injected humour, or non-characteristic words etc.”",
                        Author = "Max Maxwell"
                    });

                    db.Aphorisms.Add(new Aphorism
                    {
                        Content = @"“The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from 'de Finibus Bonorum et Malorum' 
                                  by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.”",
                        Author = "Mark Clausenberg"
                    });

                    db.SaveChanges();
                }

                if (db.BlogBanners.Any() == false)
                {
                    db.BlogBanners.Add(new BlogBanner
                    {
                        ImagePath = "breadcrumb-bg.jpg"
                    });
                }

                if (db.Blogs.Any() != true)
                {
                    db.Blogs.Add(new Blog
                    {
                        Title = "What Curling Irons Are The Best Ones",
                        Description = @"<p>
                  Hydroderm is the highly desired anti - aging cream on the block.
                  This serum restricts the occurrence of early aging sings on
                  the skin and keeps the skin younger,
                        tighter and healthier.It
                  reduces the wrinkles and loosening of skin.This cream
                  nourishes the skin and brings back the glow that had lost in
                  the run of hectic years.
                </p>
                <p>
                  The most essential ingredient that makes hydroderm so
                  effective is Vyo - Serum,
                        which is a product of natural selected
                  proteins.This concentrate works actively in bringing about
                  the natural youthful glow of the skin.It tightens the skin
                  along with its moisturizing effect on the skin.The other
                  important ingredient,
                        making hydroderm so effective is “marine
                  collagen” which along with Vyo - Serum helps revitalize the
                  skin.
                </p>",
                        ImagePath = "blog-1.jpg",
                        AphorismId = 1,
                        AuthorImagePath = "author.jfif",
                        AuthorName = "Orxan",
                        AuthorSurname = "Kərimli"
                    });

                    db.Blogs.Add(new Blog
                    {
                        Title = "Eternity Bands Do Last Forever",
                        Description = @"<p>
                  Hydroderm is the highly desired anti - aging cream on the block.
                  This serum restricts the occurrence of early aging sings on
                  the skin and keeps the skin younger,
                        tighter and healthier.It
                  reduces the wrinkles and loosening of skin.This cream
                  nourishes the skin and brings back the glow that had lost in
                  the run of hectic years.
                </p>
                <p>
                  The most essential ingredient that makes hydroderm so
                  effective is Vyo - Serum,
                        which is a product of natural selected
                  proteins.This concentrate works actively in bringing about
                  the natural youthful glow of the skin.It tightens the skin
                  along with its moisturizing effect on the skin.The other
                  important ingredient,
                        making hydroderm so effective is “marine
                  collagen” which along with Vyo - Serum helps revitalize the
                  skin.
                </p>",
                        ImagePath = "blog-2.jpg",
                        AphorismId = 2,
                        AuthorImagePath = "author.jfif",
                        AuthorName = "Orxan",
                        AuthorSurname = "Kərimli"
                    });

                    db.Blogs.Add(new Blog
                    {
                        Title = "The Health Benefits Of Sunglasses",
                        Description = @"<p>
                  Hydroderm is the highly desired anti - aging cream on the block.
                  This serum restricts the occurrence of early aging sings on
                  the skin and keeps the skin younger,
                        tighter and healthier.It
                  reduces the wrinkles and loosening of skin.This cream
                  nourishes the skin and brings back the glow that had lost in
                  the run of hectic years.
                </p>
                <p>
                  The most essential ingredient that makes hydroderm so
                  effective is Vyo - Serum,
                        which is a product of natural selected
                  proteins.This concentrate works actively in bringing about
                  the natural youthful glow of the skin.It tightens the skin
                  along with its moisturizing effect on the skin.The other
                  important ingredient,
                        making hydroderm so effective is “marine
                  collagen” which along with Vyo - Serum helps revitalize the
                  skin.
                </p>",
                        ImagePath = "blog-3.jpg",
                        AphorismId = 1,
                        AuthorImagePath = "author.jfif",
                        AuthorName = "Orxan",
                        AuthorSurname = "Kərimli"
                    });

                    db.Blogs.Add(new Blog
                    {
                        Title = "What Curling Irons Are The Best Ones",
                        Description = @"<p>
                  Hydroderm is the highly desired anti - aging cream on the block.
                  This serum restricts the occurrence of early aging sings on
                  the skin and keeps the skin younger,
                        tighter and healthier.It
                  reduces the wrinkles and loosening of skin.This cream
                  nourishes the skin and brings back the glow that had lost in
                  the run of hectic years.
                </p>
                <p>
                  The most essential ingredient that makes hydroderm so
                  effective is Vyo - Serum,
                        which is a product of natural selected
                  proteins.This concentrate works actively in bringing about
                  the natural youthful glow of the skin.It tightens the skin
                  along with its moisturizing effect on the skin.The other
                  important ingredient,
                        making hydroderm so effective is “marine
                  collagen” which along with Vyo - Serum helps revitalize the
                  skin.
                </p>",
                        ImagePath = "blog-1.jpg",
                        AphorismId = 1,
                        AuthorImagePath = "author.jfif",
                        AuthorName = "Orxan",
                        AuthorSurname = "Kərimli"
                    });

                    db.Blogs.Add(new Blog
                    {
                        Title = "Eternity Bands Do Last Forever",
                        Description = @"<p>
                  Hydroderm is the highly desired anti - aging cream on the block.
                  This serum restricts the occurrence of early aging sings on
                  the skin and keeps the skin younger,
                        tighter and healthier.It
                  reduces the wrinkles and loosening of skin.This cream
                  nourishes the skin and brings back the glow that had lost in
                  the run of hectic years.
                </p>
                <p>
                  The most essential ingredient that makes hydroderm so
                  effective is Vyo - Serum,
                        which is a product of natural selected
                  proteins.This concentrate works actively in bringing about
                  the natural youthful glow of the skin.It tightens the skin
                  along with its moisturizing effect on the skin.The other
                  important ingredient,
                        making hydroderm so effective is “marine
                  collagen” which along with Vyo - Serum helps revitalize the
                  skin.
                </p>",
                        ImagePath = "blog-2.jpg",
                        AphorismId = 2,
                        AuthorImagePath = "author.jfif",
                        AuthorName = "Orxan",
                        AuthorSurname = "Kərimli"
                    });

                    db.Blogs.Add(new Blog
                    {
                        Title = "The Health Benefits Of Sunglasses",
                        Description = @"<p>
                  Hydroderm is the highly desired anti - aging cream on the block.
                  This serum restricts the occurrence of early aging sings on
                  the skin and keeps the skin younger,
                        tighter and healthier.It
                  reduces the wrinkles and loosening of skin.This cream
                  nourishes the skin and brings back the glow that had lost in
                  the run of hectic years.
                </p>
                <p>
                  The most essential ingredient that makes hydroderm so
                  effective is Vyo - Serum,
                        which is a product of natural selected
                  proteins.This concentrate works actively in bringing about
                  the natural youthful glow of the skin.It tightens the skin
                  along with its moisturizing effect on the skin.The other
                  important ingredient,
                        making hydroderm so effective is “marine
                  collagen” which along with Vyo - Serum helps revitalize the
                  skin.
                </p>",
                        ImagePath = "blog-3.jpg",
                        AphorismId = 1,
                        AuthorImagePath = "author.jfif",
                        AuthorName = "Orxan",
                        AuthorSurname = "Kərimli"
                    });

                    db.Blogs.Add(new Blog
                    {
                        Title = "What Curling Irons Are The Best Ones",
                        Description = @"<p>
                  Hydroderm is the highly desired anti - aging cream on the block.
                  This serum restricts the occurrence of early aging sings on
                  the skin and keeps the skin younger,
                        tighter and healthier.It
                  reduces the wrinkles and loosening of skin.This cream
                  nourishes the skin and brings back the glow that had lost in
                  the run of hectic years.
                </p>
                <p>
                  The most essential ingredient that makes hydroderm so
                  effective is Vyo - Serum,
                        which is a product of natural selected
                  proteins.This concentrate works actively in bringing about
                  the natural youthful glow of the skin.It tightens the skin
                  along with its moisturizing effect on the skin.The other
                  important ingredient,
                        making hydroderm so effective is “marine
                  collagen” which along with Vyo - Serum helps revitalize the
                  skin.
                </p>",
                        ImagePath = "blog-1.jpg",
                        AphorismId = 1,
                        AuthorImagePath = "author.jfif",
                        AuthorName = "Orxan",
                        AuthorSurname = "Kərimli"
                    });

                    db.Blogs.Add(new Blog
                    {
                        Title = "Eternity Bands Do Last Forever",
                        Description = @"<p>
                  Hydroderm is the highly desired anti - aging cream on the block.
                  This serum restricts the occurrence of early aging sings on
                  the skin and keeps the skin younger,
                        tighter and healthier.It
                  reduces the wrinkles and loosening of skin.This cream
                  nourishes the skin and brings back the glow that had lost in
                  the run of hectic years.
                </p>
                <p>
                  The most essential ingredient that makes hydroderm so
                  effective is Vyo - Serum,
                        which is a product of natural selected
                  proteins.This concentrate works actively in bringing about
                  the natural youthful glow of the skin.It tightens the skin
                  along with its moisturizing effect on the skin.The other
                  important ingredient,
                        making hydroderm so effective is “marine
                  collagen” which along with Vyo - Serum helps revitalize the
                  skin.
                </p>",
                        ImagePath = "blog-2.jpg",
                        AphorismId = 2,
                        AuthorImagePath = "author.jfif",
                        AuthorName = "Orxan",
                        AuthorSurname = "Kərimli"
                    });

                    db.Blogs.Add(new Blog
                    {
                        Title = "The Health Benefits Of Sunglasses",
                        Description = @"<p>
                  Hydroderm is the highly desired anti - aging cream on the block.
                  This serum restricts the occurrence of early aging sings on
                  the skin and keeps the skin younger,
                        tighter and healthier.It
                  reduces the wrinkles and loosening of skin.This cream
                  nourishes the skin and brings back the glow that had lost in
                  the run of hectic years.
                </p>
                <p>
                  The most essential ingredient that makes hydroderm so
                  effective is Vyo - Serum,
                        which is a product of natural selected
                  proteins.This concentrate works actively in bringing about
                  the natural youthful glow of the skin.It tightens the skin
                  along with its moisturizing effect on the skin.The other
                  important ingredient,
                        making hydroderm so effective is “marine
                  collagen” which along with Vyo - Serum helps revitalize the
                  skin.
                </p>",
                        ImagePath = "blog-3.jpg",
                        AphorismId = 1,
                        AuthorImagePath = "author.jfif",
                        AuthorName = "Orxan",
                        AuthorSurname = "Kərimli"
                    });

                    db.Blogs.Add(new Blog
                    {
                        Title = "What Curling Irons Are The Best Ones",
                        Description = @"<p>
                  Hydroderm is the highly desired anti - aging cream on the block.
                  This serum restricts the occurrence of early aging sings on
                  the skin and keeps the skin younger,
                        tighter and healthier.It
                  reduces the wrinkles and loosening of skin.This cream
                  nourishes the skin and brings back the glow that had lost in
                  the run of hectic years.
                </p>
                <p>
                  The most essential ingredient that makes hydroderm so
                  effective is Vyo - Serum,
                        which is a product of natural selected
                  proteins.This concentrate works actively in bringing about
                  the natural youthful glow of the skin.It tightens the skin
                  along with its moisturizing effect on the skin.The other
                  important ingredient,
                        making hydroderm so effective is “marine
                  collagen” which along with Vyo - Serum helps revitalize the
                  skin.
                </p>",
                        ImagePath = "blog-1.jpg",
                        AphorismId = 1,
                        AuthorImagePath = "author.jfif",
                        AuthorName = "Orxan",
                        AuthorSurname = "Kərimli"
                    });

                    db.Blogs.Add(new Blog
                    {
                        Title = "Eternity Bands Do Last Forever",
                        Description = @"<p>
                  Hydroderm is the highly desired anti - aging cream on the block.
                  This serum restricts the occurrence of early aging sings on
                  the skin and keeps the skin younger,
                        tighter and healthier.It
                  reduces the wrinkles and loosening of skin.This cream
                  nourishes the skin and brings back the glow that had lost in
                  the run of hectic years.
                </p>
                <p>
                  The most essential ingredient that makes hydroderm so
                  effective is Vyo - Serum,
                        which is a product of natural selected
                  proteins.This concentrate works actively in bringing about
                  the natural youthful glow of the skin.It tightens the skin
                  along with its moisturizing effect on the skin.The other
                  important ingredient,
                        making hydroderm so effective is “marine
                  collagen” which along with Vyo - Serum helps revitalize the
                  skin.
                </p>",
                        ImagePath = "blog-2.jpg",
                        AphorismId = 2,
                        AuthorImagePath = "author.jfif",
                        AuthorName = "Orxan",
                        AuthorSurname = "Kərimli"
                    });

                    db.Blogs.Add(new Blog
                    {
                        Title = "The Health Benefits Of Sunglasses",
                        Description = @"<p>
                  Hydroderm is the highly desired anti - aging cream on the block.
                  This serum restricts the occurrence of early aging sings on
                  the skin and keeps the skin younger,
                        tighter and healthier.It
                  reduces the wrinkles and loosening of skin.This cream
                  nourishes the skin and brings back the glow that had lost in
                  the run of hectic years.
                </p>
                <p>
                  The most essential ingredient that makes hydroderm so
                  effective is Vyo - Serum,
                        which is a product of natural selected
                  proteins.This concentrate works actively in bringing about
                  the natural youthful glow of the skin.It tightens the skin
                  along with its moisturizing effect on the skin.The other
                  important ingredient,
                        making hydroderm so effective is “marine
                  collagen” which along with Vyo - Serum helps revitalize the
                  skin.
                </p>",
                        ImagePath = "blog-3.jpg",
                        AphorismId = 1,
                        AuthorImagePath = "author.jfif",
                        AuthorName = "Orxan",
                        AuthorSurname = "Kərimli"
                    });

                    db.Blogs.Add(new Blog
                    {
                        Title = "What Curling Irons Are The Best Ones",
                        Description = @"<p>
                  Hydroderm is the highly desired anti - aging cream on the block.
                  This serum restricts the occurrence of early aging sings on
                  the skin and keeps the skin younger,
                        tighter and healthier.It
                  reduces the wrinkles and loosening of skin.This cream
                  nourishes the skin and brings back the glow that had lost in
                  the run of hectic years.
                </p>
                <p>
                  The most essential ingredient that makes hydroderm so
                  effective is Vyo - Serum,
                        which is a product of natural selected
                  proteins.This concentrate works actively in bringing about
                  the natural youthful glow of the skin.It tightens the skin
                  along with its moisturizing effect on the skin.The other
                  important ingredient,
                        making hydroderm so effective is “marine
                  collagen” which along with Vyo - Serum helps revitalize the
                  skin.
                </p>",
                        ImagePath = "blog-1.jpg",
                        AphorismId = 1,
                        AuthorImagePath = "author.jfif",
                        AuthorName = "Orxan",
                        AuthorSurname = "Kərimli"
                    });

                    db.Blogs.Add(new Blog
                    {
                        Title = "Eternity Bands Do Last Forever",
                        Description = @"<p>
                  Hydroderm is the highly desired anti - aging cream on the block.
                  This serum restricts the occurrence of early aging sings on
                  the skin and keeps the skin younger,
                        tighter and healthier.It
                  reduces the wrinkles and loosening of skin.This cream
                  nourishes the skin and brings back the glow that had lost in
                  the run of hectic years.
                </p>
                <p>
                  The most essential ingredient that makes hydroderm so
                  effective is Vyo - Serum,
                        which is a product of natural selected
                  proteins.This concentrate works actively in bringing about
                  the natural youthful glow of the skin.It tightens the skin
                  along with its moisturizing effect on the skin.The other
                  important ingredient,
                        making hydroderm so effective is “marine
                  collagen” which along with Vyo - Serum helps revitalize the
                  skin.
                </p>",
                        ImagePath = "blog-2.jpg",
                        AphorismId = 2,
                        AuthorImagePath = "author.jfif",
                        AuthorName = "Orxan",
                        AuthorSurname = "Kərimli"
                    });

                    db.Blogs.Add(new Blog
                    {
                        Title = "The Health Benefits Of Sunglasses",
                        Description = @"<p>
                  Hydroderm is the highly desired anti - aging cream on the block.
                  This serum restricts the occurrence of early aging sings on
                  the skin and keeps the skin younger,
                        tighter and healthier.It
                  reduces the wrinkles and loosening of skin.This cream
                  nourishes the skin and brings back the glow that had lost in
                  the run of hectic years.
                </p>
                <p>
                  The most essential ingredient that makes hydroderm so
                  effective is Vyo - Serum,
                        which is a product of natural selected
                  proteins.This concentrate works actively in bringing about
                  the natural youthful glow of the skin.It tightens the skin
                  along with its moisturizing effect on the skin.The other
                  important ingredient,
                        making hydroderm so effective is “marine
                  collagen” which along with Vyo - Serum helps revitalize the
                  skin.
                </p>",
                        ImagePath = "blog-3.jpg",
                        AphorismId = 1,
                        AuthorImagePath = "author.jfif",
                        AuthorName = "Orxan",
                        AuthorSurname = "Kərimli"
                    });

                    db.Blogs.Add(new Blog
                    {
                        Title = "What Curling Irons Are The Best Ones",
                        Description = @"<p>
                  Hydroderm is the highly desired anti - aging cream on the block.
                  This serum restricts the occurrence of early aging sings on
                  the skin and keeps the skin younger,
                        tighter and healthier.It
                  reduces the wrinkles and loosening of skin.This cream
                  nourishes the skin and brings back the glow that had lost in
                  the run of hectic years.
                </p>
                <p>
                  The most essential ingredient that makes hydroderm so
                  effective is Vyo - Serum,
                        which is a product of natural selected
                  proteins.This concentrate works actively in bringing about
                  the natural youthful glow of the skin.It tightens the skin
                  along with its moisturizing effect on the skin.The other
                  important ingredient,
                        making hydroderm so effective is “marine
                  collagen” which along with Vyo - Serum helps revitalize the
                  skin.
                </p>",
                        ImagePath = "blog-1.jpg",
                        AphorismId = 1,
                        AuthorImagePath = "author.jfif",
                        AuthorName = "Orxan",
                        AuthorSurname = "Kərimli"
                    });

                    db.Blogs.Add(new Blog
                    {
                        Title = "Eternity Bands Do Last Forever",
                        Description = @"<p>
                  Hydroderm is the highly desired anti - aging cream on the block.
                  This serum restricts the occurrence of early aging sings on
                  the skin and keeps the skin younger,
                        tighter and healthier.It
                  reduces the wrinkles and loosening of skin.This cream
                  nourishes the skin and brings back the glow that had lost in
                  the run of hectic years.
                </p>
                <p>
                  The most essential ingredient that makes hydroderm so
                  effective is Vyo - Serum,
                        which is a product of natural selected
                  proteins.This concentrate works actively in bringing about
                  the natural youthful glow of the skin.It tightens the skin
                  along with its moisturizing effect on the skin.The other
                  important ingredient,
                        making hydroderm so effective is “marine
                  collagen” which along with Vyo - Serum helps revitalize the
                  skin.
                </p>",
                        ImagePath = "blog-2.jpg",
                        AphorismId = 2,
                        AuthorImagePath = "author.jfif",
                        AuthorName = "Orxan",
                        AuthorSurname = "Kərimli"
                    });

                    db.Blogs.Add(new Blog
                    {
                        Title = "The Health Benefits Of Sunglasses",
                        Description = @"<p>
                  Hydroderm is the highly desired anti - aging cream on the block.
                  This serum restricts the occurrence of early aging sings on
                  the skin and keeps the skin younger,
                        tighter and healthier.It
                  reduces the wrinkles and loosening of skin.This cream
                  nourishes the skin and brings back the glow that had lost in
                  the run of hectic years.
                </p>
                <p>
                  The most essential ingredient that makes hydroderm so
                  effective is Vyo - Serum,
                        which is a product of natural selected
                  proteins.This concentrate works actively in bringing about
                  the natural youthful glow of the skin.It tightens the skin
                  along with its moisturizing effect on the skin.The other
                  important ingredient,
                        making hydroderm so effective is “marine
                  collagen” which along with Vyo - Serum helps revitalize the
                  skin.
                </p>",
                        ImagePath = "blog-3.jpg",
                        AphorismId = 1,
                        AuthorImagePath = "author.jfif",
                        AuthorName = "Orxan",
                        AuthorSurname = "Kərimli"
                    });

                    db.SaveChanges();
                }

                if (db.Tags.Any() != true)
                {
                    db.Tags.Add(new Tag
                    {
                        TagName = "2020"
                    });

                    db.Tags.Add(new Tag
                    {
                        TagName = "Fashion"
                    });

                    db.Tags.Add(new Tag
                    {
                        TagName = "Lifestyle"
                    });

                    db.Tags.Add(new Tag
                    {
                        TagName = "Brand"
                    });

                    db.Tags.Add(new Tag
                    {
                        TagName = "New"
                    });

                    db.Tags.Add(new Tag
                    {
                        TagName = "Latest"
                    });

                    db.SaveChanges();
                }

                if (db.BlogDetailsTagsCollections.Any() == false)
                {
                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 1,
                        TagId = 1
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 1,
                        TagId = 2
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 1,
                        TagId = 3
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 2,
                        TagId = 4
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 2,
                        TagId = 5
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 2,
                        TagId = 6
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 3,
                        TagId = 3
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 3,
                        TagId = 4
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 3,
                        TagId = 6
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 4,
                        TagId = 1
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 4,
                        TagId = 2
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 4,
                        TagId = 3
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 5,
                        TagId = 4
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 5,
                        TagId = 5
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 5,
                        TagId = 6
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 6,
                        TagId = 3
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 6,
                        TagId = 4
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 6,
                        TagId = 6
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 7,
                        TagId = 1
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 7,
                        TagId = 2
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 7,
                        TagId = 3
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 8,
                        TagId = 4
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 8,
                        TagId = 5
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 8,
                        TagId = 6
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 9,
                        TagId = 3
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 9,
                        TagId = 4
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 9,
                        TagId = 6
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 10,
                        TagId = 1
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 10,
                        TagId = 2
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 10,
                        TagId = 3
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 11,
                        TagId = 4
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 11,
                        TagId = 5
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 11,
                        TagId = 6
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 12,
                        TagId = 3
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 12,
                        TagId = 4
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 12,
                        TagId = 6
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 13,
                        TagId = 1
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 13,
                        TagId = 2
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 13,
                        TagId = 3
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 14,
                        TagId = 4
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 14,
                        TagId = 5
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 14,
                        TagId = 6
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 15,
                        TagId = 3
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 15,
                        TagId = 4
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 15,
                        TagId = 6
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 16,
                        TagId = 1
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 16,
                        TagId = 2
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 16,
                        TagId = 3
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 17,
                        TagId = 4
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 17,
                        TagId = 5
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 17,
                        TagId = 6
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 18,
                        TagId = 3
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 18,
                        TagId = 4
                    });

                    db.BlogDetailsTagsCollections.Add(new BlogDetailsTagsCollection
                    {
                        BlogId = 18,
                        TagId = 6
                    });

                    db.SaveChanges();
                }

                if (db.Contacts.Any() != true)
                {
                    db.Contacts.Add(new Contact
                    {
                        Title = "Contact Us",
                        Description = "As you might expect of a company that began as a high-end  interiors contractor, we pay strict attention."
                    });

                    db.SaveChanges();
                }

                if (db.Departments.Any() != true)
                {
                    db.Departments.Add(new Department
                    {
                        City = "Baku City",
                        Address = "28 May",
                        PhoneNumber = "+9946393239",
                        ContactId = 1
                    });

                    db.Departments.Add(new Department
                    {
                        City = "Ganja City",
                        Address = "Baghbanlar Street",
                        PhoneNumber = "+208911300",
                        ContactId = 1
                    });

                    db.SaveChanges();
                }

                if (!db.AboutUsBanners.Any())
                {
                    db.AboutUsBanners.Add(new AboutUsBanner
                    {
                        ImagePath = "about-us.jpg"
                    });

                    db.SaveChanges();
                }

                if (db.TeamJobs.Any() == false)
                {
                    db.TeamJobs.Add(new TeamJob
                    {
                        JobName = "Fashion Design"
                    });

                    db.TeamJobs.Add(new TeamJob
                    {
                        JobName = "C.E.O"
                    });

                    db.TeamJobs.Add(new TeamJob
                    {
                        JobName = "Manager"
                    });

                    db.TeamJobs.Add(new TeamJob
                    {
                        JobName = "Delivery"
                    });

                    db.SaveChanges();
                }

                if (db.Teams.Any() != true)
                {
                    db.Teams.Add(new Team
                    {
                        ImagePath = "team-1.jpg",
                        Name = "John",
                        Surname = "Smith",
                        TeamJobId = 1
                    });

                    db.Teams.Add(new Team
                    {
                        ImagePath = "team-2.jpg",
                        Name = "Christine",
                        Surname = "Wise",
                        TeamJobId = 2
                    });

                    db.SaveChanges();
                }

                if (!db.HappyClients.Any())
                {
                    db.HappyClients.Add(new HappyClient
                    {
                        ImagePath = "client-1.png"
                    });

                    db.HappyClients.Add(new HappyClient
                    {
                        ImagePath = "client-2.png"
                    });

                    db.HappyClients.Add(new HappyClient
                    {
                        ImagePath = "client-3.png"
                    });

                    db.HappyClients.Add(new HappyClient
                    {
                        ImagePath = "client-4.png"
                    });

                    db.HappyClients.Add(new HappyClient
                    {
                        ImagePath = "client-5.png"
                    });

                    db.HappyClients.Add(new HappyClient
                    {
                        ImagePath = "client-6.png"
                    });

                    db.HappyClients.Add(new HappyClient
                    {
                        ImagePath = "client-7.png"
                    });

                    db.HappyClients.Add(new HappyClient
                    {
                        ImagePath = "client-8.png"
                    });

                    db.SaveChanges();
                }

                if (db.WhyWes.Any() == false)
                {
                    db.WhyWes.Add(new WhyWe
                    {
                        Title = "Who We Are?",
                        Description = "Contextual advertising programs sometimes have strict policies that need to be adhered too.Let’s take Google as an example."
                    });

                    db.WhyWes.Add(new WhyWe
                    {
                        Title = "Who We Do?",
                        Description = "In this digital generation where information can be easily obtained within seconds, business cards still have retained their importance."
                    });

                    db.WhyWes.Add(new WhyWe
                    {
                        Title = "Why Choose Us?",
                        Description = " A two or three storey house is the ideal way to maximise the piece of earth on which our home sits, but for older or infirm people."
                    });

                    db.SaveChanges();
                }

                if (db.Brands.Any() == false)
                {
                    db.Brands.Add(new Brand
                    {
                        BrandName = "Louis Vuitton"
                    });


                    db.Brands.Add(new Brand
                    {
                        BrandName = "Chanel"
                    });


                    db.Brands.Add(new Brand
                    {
                        BrandName = "Hermes"
                    });


                    db.Brands.Add(new Brand
                    {
                        BrandName = "Gucci"
                    });

                    db.SaveChanges();
                }

                if (!db.Categories.Any())
                {
                    db.Categories.Add(new Category
                    {
                        Name = "Men"
                    });

                    db.Categories.Add(new Category
                    {
                        Name = "Women"
                    });

                    db.Categories.Add(new Category
                    {
                        Name = "Bags"
                    });

                    db.Categories.Add(new Category
                    {
                        Name = "Clothing"
                    });

                    db.Categories.Add(new Category
                    {
                        Name = "Shoes"
                    });

                    db.Categories.Add(new Category
                    {
                        Name = "Accessoires"
                    });

                    db.Categories.Add(new Category
                    {
                        Name = "Kids"
                    });

                    db.SaveChanges();
                }

                if (db.Sizes.Any() != true)
                {
                    db.Sizes.Add(new Size
                    {
                        SizeName = "XS"
                    });

                    db.Sizes.Add(new Size
                    {
                        SizeName = "S"
                    });

                    db.Sizes.Add(new Size
                    {
                        SizeName = "M"
                    });

                    db.Sizes.Add(new Size
                    {
                        SizeName = "XL"
                    });

                    db.Sizes.Add(new Size
                    {
                        SizeName = "XXL"
                    });

                    db.Sizes.Add(new Size
                    {
                        SizeName = "3XL"
                    });

                    db.Sizes.Add(new Size
                    {
                        SizeName = "4XL"
                    });

                    db.SaveChanges();
                }

                if (!db.Colors.Any())
                {
                    db.Colors.Add(new Color
                    {
                        ColorName = "Black"
                    });

                    db.Colors.Add(new Color
                    {
                        ColorName = "Blue"
                    });

                    db.Colors.Add(new Color
                    {
                        ColorName = "Yellow"
                    });

                    db.Colors.Add(new Color
                    {
                        ColorName = "Grey"
                    });

                    db.Colors.Add(new Color
                    {
                        ColorName = "Green"
                    });

                    db.Colors.Add(new Color
                    {
                        ColorName = "Pink"
                    });

                    db.Colors.Add(new Color
                    {
                        ColorName = "Light Purple"
                    });

                    db.Colors.Add(new Color
                    {
                        ColorName = "Red"
                    });

                    db.Colors.Add(new Color
                    {
                        ColorName = "White"
                    });

                    db.SaveChanges();
                }

                if (db.ProductTags.Any() == false)
                {
                    db.ProductTags.Add(new ProductTag
                    {
                        ProductTagName = "Product"
                    });

                    db.ProductTags.Add(new ProductTag
                    {
                        ProductTagName = "Bags"
                    });

                    db.ProductTags.Add(new ProductTag
                    {
                        ProductTagName = "Shoes"
                    });

                    db.ProductTags.Add(new ProductTag
                    {
                        ProductTagName = "Fashion"
                    });

                    db.ProductTags.Add(new ProductTag
                    {
                        ProductTagName = "Clothing"
                    });

                    db.ProductTags.Add(new ProductTag
                    {
                        ProductTagName = "Hats"
                    });

                    db.ProductTags.Add(new ProductTag
                    {
                        ProductTagName = "Accessoires"
                    });

                    db.SaveChanges();
                }

                if (db.Campaigns.Any() != true)
                {
                    db.Campaigns.Add(new Campaign
                    {
                        Title = "Novruz Bayramı Endirimi",
                        Description = "Bizdən alın, razı qalın! (Novruz)",
                        Discount = 25.00M,
                        ExpiredDate = DateTime.Now.AddDays(25),
                        IsApproved = true
                    });

                    db.Campaigns.Add(new Campaign
                    {
                        Title = "Yeni İl Endirimi",
                        Description = "Bizdən alın, razı qalın! (Yeni İl)",
                        Discount = 50.00M,
                        ExpiredDate = DateTime.Now.AddDays(35),
                        IsApproved = false
                    });

                    db.SaveChanges();
                }
            }

            return app;
        }
    }
}
