using MaleFashion.eCommerce.WebUI.Models.Entity;
using MaleFashion.eCommerce.WebUI.Models.Entity.Membership;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.DataContext
{
    public class FashionDbContext :
    IdentityDbContext<AppUser, AppRole, int, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
    {
        public FashionDbContext(DbContextOptions options)
            : base(options)
        {

        }

        //IDENTITY
        public DbSet<AppUser> Users { get; set; }
        public DbSet<AppRole> Roles { get; set; }
        public DbSet<AppUserClaim> UserClaims { get; set; }
        public DbSet<AppUserRole> UserRoles { get; set; }
        public DbSet<AppUserLogin> UserLogins { get; set; }
        public DbSet<AppRoleClaim> RoleClaims { get; set; }
        public DbSet<AppUserToken> UserTokens { get; set; }
        //IDENTITY

        // ---INJECT---
        public DbSet<AppInfo> AppInfos { get; set; }
        // ---INJECT---
        // ---SHOP---
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<ProductMainCollection> ProductMainCollections { get; set; }
        // ---SHOP---
        // ---BLOG---
        public DbSet<Aphorism> Aphorisms { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogBanner> BlogBanners { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogDetailsTagsCollection> BlogDetailsTagsCollections { get; set; }
        //---COMMENT-REPLY-LIKE-UNLIKE---
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Unlike> Unlikes { get; set; }
        //---COMMENT-REPLY-LIKE-UNLIKE---
        // ---BLOG---
        // ---CONTACTS---
        public DbSet<Department> Departments { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        // ---CONTACTS---
        // ---ABOUT-US---
        public DbSet<AboutUsBanner> AboutUsBanners { get; set; }
        public DbSet<TeamJob> TeamJobs { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<HappyClient> HappyClients { get; set; }
        public DbSet<WhyWe> WhyWes { get; set; }
        // ---ABOUT-US---
        //---MAPS---
        public DbSet<Map> Maps { get; set; }
        //---MAPS---
        //---DISCOUNT---
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<ProductCampaignCollection> ProductCampaignCollections { get; set; }
        //---DISCOUNT---
        //---SUBSCRIPTION---
        public DbSet<Subscription> Subscriptions { get; set; }
        //---SUBSCRIPTION---

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // If we want to add new schema to our database, so we can use this code...
            //builder.HasDefaultSchema("FSHN");

            // ---INJECT---
            builder.Entity<AppInfo>()
                   .Property(ap => ap.CreatedDate)
                   .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");
            // ---INJECT---

            // ---SHOP---
            builder.Entity<Category>()
                   .Property(ap => ap.CreatedDate)
                   .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");

            builder.Entity<Brand>()
                   .Property(ap => ap.CreatedDate)
                   .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");

            builder.Entity<Product>()
                   .Property(ap => ap.CreatedDate)
                   .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");

            builder.Entity<Size>()
                   .Property(ap => ap.CreatedDate)
                   .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");

            builder.Entity<Color>()
                   .Property(ap => ap.CreatedDate)
                   .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");

            builder.Entity<ProductTag>()
                   .Property(ap => ap.CreatedDate)
                   .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");

            builder.Entity<Campaign>()
                   .Property(ap => ap.CreatedDate)
                   .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");
            // ---SHOP---

            // ---BLOG---
            builder.Entity<Aphorism>()
                   .Property(ap => ap.CreatedDate)
                   .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");

            builder.Entity<Tag>()
                   .Property(ap => ap.CreatedDate)
                   .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");

            builder.Entity<BlogBanner>()
                 .Property(ap => ap.CreatedDate)
                 .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");

            builder.Entity<Blog>()
                   .Property(ap => ap.CreatedDate)
                   .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");
            // ---BLOG---

            // ---CONTACTS---
            builder.Entity<Department>()
                   .Property(ap => ap.CreatedDate)
                   .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");

            builder.Entity<Contact>()
                   .Property(ap => ap.CreatedDate)
                   .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");
            // ---CONTACTS---

            // ---ABOUT-US---
            builder.Entity<AboutUsBanner>()
              .Property(ap => ap.CreatedDate)
              .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");

            builder.Entity<TeamJob>()
              .Property(ap => ap.CreatedDate)
              .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");

            builder.Entity<Team>()
              .Property(ap => ap.CreatedDate)
              .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");

            builder.Entity<HappyClient>()
              .Property(ap => ap.CreatedDate)
              .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");

            builder.Entity<WhyWe>()
           .Property(ap => ap.CreatedDate)
           .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");
            // ---ABOUT-US---

            //---MAP---
            builder.Entity<Map>()
            .Property(ap => ap.CreatedDate)
            .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");
            //---MAP---

            //IDENTITY ToTable
            builder.Entity<AppUser>(e =>
            {
                e.ToTable("Users", "Membership");
            });

            builder.Entity<AppRole>(e =>
            {
                e.ToTable("Roles", "Membership");
            });

            builder.Entity<AppUserRole>(e =>
            {
                e.ToTable("UserRoles", "Membership");
            });

            builder.Entity<AppUserClaim>(e =>
            {
                e.ToTable("UserClaims", "Membership");
            });

            builder.Entity<AppRoleClaim>(e =>
            {
                e.ToTable("RoleClaims", "Membership");
            });

            builder.Entity<AppUserToken>(e =>
            {
                e.ToTable("UserTokens", "Membership");
            });

            builder.Entity<AppUserLogin>(e =>
            {
                e.ToTable("UserLogins", "Membership");
            });
            //IDENTITY ToTable
        }
    }
}
