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
        //[NonAction]
        //public IEnumerable<SelectListItem> GetAllCategories()
        //{
        //    // generam o lista goala
        //    var selectList = new List<SelectListItem>();
        //    // Extragem toate categoriile din baza de date
        //    var categories = from cat in db.Categories select cat;
        //    // iteram prin categorii
        //    foreach (var category in categories)
        //    {
        //        // Adaugam in lista elementele necesare pentru dropdown
        //        selectList.Add(new SelectListItem
        //        {
        //            Value = category.CategoryId.ToString(),
        //            Text = category.CategoryName.ToString()
        //        });
        //    }
        //    // returnam lista de categorii
        //    return selectList;
        //}
        [Authorize(Roles = "RegisteredUser,Administrator")]
        public ActionResult New()
        {
            Bookmark bookmark = new Bookmark();
            // preluam lista de tag-uri din metoda GetAllCategories()  
         //   bookmark.Tags = GetAllCategories();
            // Preluam ID-ul utilizatorului curent 
            bookmark.UserId = User.Identity.GetUserId();
            return View(bookmark);
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New(Bookmark article)
        {
            //article.Categories = GetAllCategories();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Bookmarks.Add(article);
                    db.SaveChanges();
                    TempData["message"] = "Articolul a fost adaugat!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(article);
                }
            }
            catch (Exception e)
            {
                return View(article);
            }
        }
    }
}