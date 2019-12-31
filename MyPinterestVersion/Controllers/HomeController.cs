using MyPinterestVersion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using System.Web;
using System.Web.Mvc;

namespace MyPinterestVersion.Controllers
{
    
    public class HomeController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        public ActionResult Index(int? page)
        {
            var bookmarks = db.Bookmarks.Include("Image").ToList<Bookmark>();
            int num = bookmarks.Count();
            for (int i=0;i<num;i++)
            {
                
                var temp = bookmarks[i].Id;
                var Tags = db.BookmarkTagLinks.Where(c => c.BookmarkId == temp).ToList<BookmarkTagLink>();

                //if (Tags.Capacity == 0)
                //{
                //    bookmarks[i].TagsNames = new List<string>(1);
                //    bookmarks[i].TagsNames.Add("test");
                //}
                bookmarks[i].TagsNames = new List<string>();
                foreach (BookmarkTagLink tag in Tags)
                {
                    //System.Diagnostics.Debug.WriteLine(tag.Id);
                    var TagsFull = db.Tags.Where(c => c.Id == tag.TagId).Select(c => c.Name).ToList<string>();
                    
                    bookmarks[i].TagsNames.Add(TagsFull[0]);
                }
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(bookmarks.ToPagedList(pageNumber, pageSize));
            //return View(bookmarks);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}