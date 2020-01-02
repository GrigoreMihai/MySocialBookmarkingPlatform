using Microsoft.AspNet.Identity;
using MyPinterestVersion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPinterestVersion.Controllers
{
    [Authorize(Roles = "RegisteredUser,Administrator")]
    public class BookmarkController : Controller
    {
        
        private ApplicationDbContext db = ApplicationDbContext.Create();
        // GET: Bookmark
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult VoteUp(Bookmark bookmark)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Bookmark book = db.Bookmarks.Find(bookmark.Id);
                    if (TryUpdateModel(book))
                    {
                        book.Note++;
                        db.SaveChanges();
                        TempData["message"] = "Articolul a fost modificat!";
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Show", new { id = bookmark.Id });
            }
            return RedirectToAction("Show", new { id = bookmark.Id });
        }
        
        public ActionResult AddComment(Bookmark bookmark)
        {
            System.Diagnostics.Debug.WriteLine("jshdhjs");
            try
            {
                if (ModelState.IsValid)
                {
                    Comment comment = new Comment { CommentBody = bookmark.Comment, BookmarkId = bookmark.Id };
                    comment.CommentBody = bookmark.Comment;
                    db.Comments.Add(comment);
                    db.SaveChanges();
                    TempData["message"] = "Your comment has been added";
                    return RedirectToAction("Show", new { id = bookmark.Id });
                                    
                }
                
            }
            catch (Exception e)
            {
                return RedirectToAction("Show", new { id = bookmark.Id});
            }
            return RedirectToAction("Show", new { id = bookmark.Id });
        }
        [NonAction]
        public List<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            // Extragem toate categoriile din baza de date
            var tags = from tag in db.Tags select tag;
            // iteram prin categorii
            foreach (var tag in tags)
            {
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Text = tag.Name.ToString(),
                    Value = tag.Id.ToString()
                    
                });
            }
            // returnam lista de categorii
            return selectList;
        }
        [Authorize(Roles = "RegisteredUser,Administrator")]
        public ActionResult New(string Id)
        {

            // preluam lista de tag-uri din metoda GetAllCategories()  

            Bookmark bookmark = new Bookmark();
            bookmark.Tags = GetAllCategories();
            // Preluam ID-ul utilizatorului curent 
            bookmark.UserId = User.Identity.GetUserId();
            bookmark.ImageId = Int32.Parse(Id);
            return View(bookmark);
        }

        [HttpPost]
        [Authorize(Roles = "RegisteredUser,Administrator")]
        public ActionResult New(Bookmark bookmark)
        { 
            //bookmark.Tags = GetAllCategories();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Bookmarks.Add(bookmark);
                    db.SaveChanges();
                    BookmarkTagLink tagBookLink = new BookmarkTagLink();
                    foreach (SelectListItem item in bookmark.Tags)
                    {
                        if (item.Selected)
                        {

                            tagBookLink.TagId = Int32.Parse(item.Value);
                            tagBookLink.BookmarkId = bookmark.Id;
                            db.BookmarkTagLinks.Add(tagBookLink);
                            db.SaveChanges();
                        }
                   
                    
                    }
                    TempData["message"] = "Bookmark has been added!";
                    
                    return RedirectToAction("../Manage/ViewManageBookmark");
                }
                else
                {
                    return View(bookmark);
                } 
            }
            catch (Exception e)
            {               
                return View(bookmark);
            }
        }
        [Authorize(Roles = "RegisteredUser,Administrator")]
        public ActionResult Show(int id)
        {
            var bookmark = db.Bookmarks.Include("Image").SingleOrDefault(x => x.Id == id);




            var temp = id;
                var Tags = db.BookmarkTagLinks.Where(c => c.BookmarkId == temp).ToList<BookmarkTagLink>();
               
                bookmark.TagsNames = new List<string>();
                
                foreach (BookmarkTagLink tag in Tags)
                {

                    var TagsFull = db.Tags.Where(c => c.Id == tag.TagId).Select(c => c.Name).ToList<string>();
                   
                    bookmark.TagsNames.Add(TagsFull[0]);
                }
                
            

           

            ViewBag.esteAdmin = User.IsInRole("Administrator"); ViewBag.utilizatorCurent = User.Identity.GetUserId();

            return View(bookmark);
        }
    }
}