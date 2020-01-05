using MyPinterestVersion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPinterestVersion.Controllers
{
    public class CommentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }
        [HttpDelete]
        public ActionResult Delete(int id,int ?bookId)
        {
            Comment category = db.Comments.Find(id);
            db.Comments.Remove(category);
            TempData["message"] = "The comment has been deleted!";
            db.SaveChanges();
            return RedirectToAction("../Bookmark/Show", new { id = bookId });
        }
    }
}