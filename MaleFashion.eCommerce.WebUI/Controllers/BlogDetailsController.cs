using MaleFashion.eCommerce.WebUI.AppCode.Extensions;
using MaleFashion.eCommerce.WebUI.Models.DataContext;
using MaleFashion.eCommerce.WebUI.Models.Entity;
using MaleFashion.eCommerce.WebUI.Models.Entity.Membership;
using MaleFashion.eCommerce.WebUI.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Controllers
{
    public class BlogDetailsController : Controller
    {

        private readonly FashionDbContext db;

        public BlogDetailsController(FashionDbContext db)
        {
            this.db = db;
        }

        [Route("[Controller]/{id}/blogs/{title}")]
        [AllowAnonymous]
        public IActionResult Index(int? id)
        {
            BlogDetailsViewModel viewModel = new BlogDetailsViewModel();

            viewModel.BlogDetailsTagsCollections = db.BlogDetailsTagsCollections
                                                   .Include(b => b.Blog)
                                                   .Include(a => a.Blog.Aphorism)
                                                   .Include(t => t.Tag)
                                                   .Where(b => b.Blog.DeletedDate == null)
                                                   .Where(b => b.Blog.Aphorism.DeletedDate == null)
                                                   .Where(b => b.Tag.DeletedDate == null)
                                                   .Where(b => b.Blog.Id.Equals(id))
                                                   .ToList();

            viewModel.Comments = db.Comments.Include(c => c.Replies).Where(c => c.BlogId == id).ToList();

            viewModel.Replies = db.Replies.Where(r => r.BlogId == id).ToList();

            viewModel.Likes = db.Likes.Where(l => l.BlogId == id).ToList();

            viewModel.Unlikes = db.Unlikes.Where(u => u.BlogId == id).ToList();

            Blog prev = db.Blogs.FirstOrDefault(b => b.Id == (id - 1) && b.DeletedDate == null);
            Blog next = db.Blogs.FirstOrDefault(b => b.Id == (id + 1) && b.DeletedDate == null);

            viewModel.PrevBlog = prev;

            viewModel.NextBlog = next;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        async public Task<IActionResult> Comment([Bind("BlogId, Content")] Comment comment)
        {
            AppUser currentUser = await db.Users.FirstOrDefaultAsync(u => u.Id == User.GetUserId());
            int userId = User.GetUserId();

            Comment commentResult = new Comment
            {
                BlogId = comment.BlogId,
                Content = comment.Content,
                AuthorImagePath = currentUser.ImagePath,
                AuthorName = currentUser.Name,
                AuthorSurname = currentUser.Surname,
                UserId = userId
            };

            try
            {
                await db.Comments.AddAsync(commentResult);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Json(new
                {
                    error = true,
                    message = "Xeta bash verdi, bir-neche deqiqeden sonra yeniden cehd edin!"
                });
            }

            return Json(new
            {
                error = false,
                message = ""
            });
        }

        [HttpPost]
        [Authorize]
        async public Task<IActionResult> Reply(int replyBlogId, int replyCommentId, string replyContent)
        {
            AppUser currentUser = await db.Users.FirstOrDefaultAsync(u => u.Id == User.GetUserId());
            int userId = User.GetUserId();

            Reply replyResult = new Reply
            {
                CommentId = replyCommentId,
                BlogId = replyBlogId,
                Content = replyContent,
                AuthorImagePath = currentUser.ImagePath,
                AuthorName = currentUser.Name,
                AuthorSurname = currentUser.Surname,
                UserId = userId
            };

            try
            {
                await db.Replies.AddAsync(replyResult);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Json(new
                {
                    error = true,
                    message = "Xeta bash verdi, bir-neche deqiqeden sonra yeniden cehd edin!"
                });
            }

            return Json(new
            {
                error = false,
                message = ""
            });
        }
    }
}
