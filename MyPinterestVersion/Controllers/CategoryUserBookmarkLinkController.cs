using Microsoft.AspNet.Identity;
using MyPinterestVersion.Models;
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
        public ActionResult Index()
        {
            return View();
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
                    return View(book);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return View(book);
            }

        }
    }
}