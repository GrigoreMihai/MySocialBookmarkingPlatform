using Microsoft.AspNet.Identity;
using MyPinterestVersion.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPinterestVersion.Controllers
{
    public class CategoryUserBookmarkLinkController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [NonAction]
        public List<SelectListItem> GetAllCategoriesReally()
        {

            var selectList = new List<SelectListItem>();

            var tags = from tag in db.Categories select tag;

            foreach (var tag in tags)
            {

                selectList.Add(new SelectListItem
                {
                    Text = tag.Name.ToString(),
                    Value = tag.Id.ToString()

                });
            }
            // returnam lista de categorii
            return selectList;
        }
        // GET: CategoryUserBookmarkLink
        public ActionResult Index(int? page)
        {
            var userId = User.Identity.GetUserId();
            var bookCat = db.CategoryUserBookmarkLinks.Where(m => m.UserId == userId).ToList<CategoryUserBookmarkLink>();
            List<Bookmark> bookmarks = new List<Bookmark>();
            foreach (CategoryUserBookmarkLink p in bookCat)
            {
                var id = p.BookmarkId;
                var catId = p.CategoryId;
                var cat = db.Categories.Where(m => m.Id == catId).Select(m => m.Name).ToList<string>();

                var bookmark = db.Bookmarks.Include("Image").SingleOrDefault(x => x.Id == id);
                var comments = db.Comments.Where(m => m.BookmarkId == id);
                bookmark.CategoryName = cat[0];
                if (comments.Count() > 0)
                {
                    bookmark.CommentsList = new List<Comment>();
                    bookmark.CommentsList = comments.ToList<Comment>();
                    ViewBag.hasComments = true;
                }
                var urls = db.SimilarUrls.Where(m => m.BookmarkId == id);

                if (urls.Count() > 0)
                {
                    bookmark.SimilarUrls = new List<SimilarUrl>();
                    bookmark.SimilarUrls = urls.ToList<SimilarUrl>();
                    ViewBag.hasUrl = true;
                }
                if (TempData.ContainsKey("message"))
                {
                    ViewBag.message = TempData["message"].ToString();
                }



                var temp = id;
                var Tags = db.BookmarkTagLinks.Where(c => c.BookmarkId == temp).ToList<BookmarkTagLink>();

                bookmark.TagsNames = new List<string>();

                foreach (BookmarkTagLink tag in Tags)
                {

                    var TagsFull = db.Tags.Where(c => c.Id == tag.TagId).Select(c => c.Name).ToList<string>();

                    bookmark.TagsNames.Add(TagsFull[0]);
                }
                bookmarks.Add(bookmark);
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(bookmarks.ToPagedList(pageNumber, pageSize));          
                
            
            
        }
        [Authorize(Roles = "RegisteredUser,Administrator")]
        public ActionResult Save(int id)
        {
            CategoryUserBookmarkLink savedCategoryBook = new CategoryUserBookmarkLink();
            savedCategoryBook.Categories = GetAllCategoriesReally();
            savedCategoryBook.BookmarkId = id;
            savedCategoryBook.UserId = User.Identity.GetUserId();
            return View(savedCategoryBook);

        }
        [HttpPost]
        [Authorize(Roles = "RegisteredUser,Administrator")]
        public ActionResult Save(CategoryUserBookmarkLink book)
        {
           
            
            try
            {
                if (ModelState.IsValid)
                {
                    db.CategoryUserBookmarkLinks.Add(book);
                    db.SaveChanges();                         
                    TempData["message"] = "Bookmark has been saved!";
                    return RedirectToAction("Index");
                }
                else
                {
                    book.Categories = GetAllCategoriesReally();
                    return View(book);
                }
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(e);
                book.Categories = GetAllCategoriesReally();
                return View(book);
            }

        }
    }
}