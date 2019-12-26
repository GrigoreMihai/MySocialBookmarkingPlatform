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
        // GET: Bookmark
        public ActionResult Index()
        {
            return View();
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
                if (ModelState.IsValid || 1!=0)
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
                    TempData["message"] = "Articolul a fost adaugat!";
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
    }
}