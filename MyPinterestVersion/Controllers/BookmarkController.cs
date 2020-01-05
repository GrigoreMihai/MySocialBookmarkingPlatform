using Microsoft.AspNet.Identity;
using MyPinterestVersion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPinterestVersion.Controllers
{
   
    public class BookmarkController : Controller
    {
        
        private ApplicationDbContext db = ApplicationDbContext.Create();
      
        public ActionResult Search(string SearchString, string by)
        {
            
            if (!String.IsNullOrEmpty(SearchString))
            {
                ViewBag.SearchString = SearchString;
                ViewBag.by = by;
                return RedirectToAction("../Home/Index", new { SearchString = SearchString, by=by, sortOrder = "Date" });
            }
            return View();
        }       
       

        [Authorize(Roles = "RegisteredUser,Administrator")]
        public ActionResult VoteUp(int id)
        {
            try
            {              
                    Bookmark book = db.Bookmarks.Find(id);
                    if (TryUpdateModel(book))
                    {
                        book.Note++;
                        db.SaveChanges();
                        TempData["message"] = "Your vote has been added!";
                    }
                    return RedirectToAction("Show", new { id = id });
                
            }
            catch (Exception e)
            {
                return RedirectToAction("Show", new { id = id });
            }
           
        }
        [Authorize(Roles = "RegisteredUser,Administrator")]
        public ActionResult VoteDown(int id)
        {
            try
            {
               
                 Bookmark book = db.Bookmarks.Find(id);
                 if (TryUpdateModel(book))
                 {
                     book.Note--;
                     db.SaveChanges();
                     TempData["message"] = "Your vote has been added!";
                 }
                 return RedirectToAction("Show", new { id = id });
               
            }
            catch (Exception e)
            {
                return RedirectToAction("Show", new { id = id });
            }
            
        }
        [HttpPost]
        [Authorize(Roles = "RegisteredUser,Administrator")]
        public ActionResult AddComment(Bookmark bookmark)
        {
           
            try
            {          
                    Comment comment = new Comment();
                    comment.BookmarkId = bookmark.Id;
                    System.Diagnostics.Debug.WriteLine(bookmark.Comment);
                    comment.CommentBody = bookmark.Comment;
                    comment.UserId = User.Identity.GetUserId();
                    comment.Date = bookmark.CommentDate;
                    db.Comments.Add(comment);
                    db.SaveChanges();                    
                    TempData["message"] = "Your comment has been added";                   
                    return RedirectToAction("Show", new { id = bookmark.Id });
            }
            catch (Exception e)
            {
                
                return RedirectToAction("Show", new { id = bookmark.Id});
            }
            
        }
        [HttpPost]
        [Authorize(Roles = "RegisteredUser,Administrator")]
        public ActionResult AddUrl(Bookmark bookmark)
        {

            try
            {
                SimilarUrl url = new SimilarUrl();
                url.BookmarkId = bookmark.Id;
                System.Diagnostics.Debug.WriteLine(bookmark.Comment);
                url.UrlBody = bookmark.Url;
                url.UserId = User.Identity.GetUserId();
                url.Date = bookmark.SimilarUrlDate;
                db.SimilarUrls.Add(url);
                db.SaveChanges();
                TempData["message"] = "Your related url has been added";
                return RedirectToAction("Show", new { id = bookmark.Id });
            }
            catch (Exception e)
            {

                return RedirectToAction("Show", new { id = bookmark.Id });
            }

        }
        [NonAction]
        public List<SelectListItem> GetAllCategories() //GetAllTags
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
            var comments = db.Comments.Where(m => m.BookmarkId == id);
            
            if (comments.Count() > 0 )
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
                
            

           

            ViewBag.esteAdmin = User.IsInRole("Administrator"); ViewBag.utilizatorCurent = User.Identity.GetUserId();
            
            
            
            return View(bookmark);
        }
        [HttpDelete]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            Bookmark article = db.Bookmarks.Find(id);
            List<Comment> com = db.Comments.Where(m => m.BookmarkId == id).ToList<Comment>();
            List<SimilarUrl> sim = db.SimilarUrls.Where(m => m.BookmarkId == id).ToList<SimilarUrl>();
            foreach (var c in com)
            {
                db.Comments.Remove(c);
            }
            foreach (var c in sim)
            {
                db.SimilarUrls.Remove(c);
            }
            db.Bookmarks.Remove(article);
             db.SaveChanges();
             TempData["message"] = "Bookmark-ul a fost sters!";
             return RedirectToAction("../Home/Index");
          
        

        }
    }
}