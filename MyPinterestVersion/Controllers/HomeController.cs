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
      
        public ActionResult Index(int? page, string SearchString, string by, string sortOrder)
        {
            var bookmarks = db.Bookmarks.Include("Image").ToList<Bookmark>();
            if (!String.IsNullOrEmpty(SearchString) && !String.IsNullOrEmpty(by))
            {
                if (string.Compare("title", by) == 0)
                {
                    bookmarks = db.Bookmarks.Include("Image").Where(m => (string.Compare(m.Title, SearchString) == 0)).ToList<Bookmark>();
                }
                if (string.Compare("description", by) == 0)
                {
                    bookmarks = db.Bookmarks.Include("Image").Where(m => m.Description.Contains(SearchString)).ToList<Bookmark>();
                }
            }


            ViewBag.RatingSortParm = String.IsNullOrEmpty(sortOrder) ? "rating_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["rating_desc"] = true;
            ViewData["date_desc"] = true;
            switch (sortOrder)
            {
                case "rating_desc":
                    {
                        ViewData["rating_desc"] = true;
                        bookmarks.Sort((t1, t2) => t1.Note.CompareTo(t2.Note));
                    }
                    break;
                case "Date":
                    {
                        ViewData["date_desc"] = false;
                        bookmarks.Sort((t2, t1) => t1.Date.CompareTo(t2.Date));

                    }
                    break;
                case "date_desc":
                    {
                        ViewData["date_desc"] = true;
                        bookmarks.Sort((t1, t2) => t1.Date.CompareTo(t2.Date));
                    }
                    break;
                default:
                    {
                        ViewData["rating_desc"] = false;
                        bookmarks.Sort((t2, t1) => t1.Note.CompareTo(t2.Note));
                    }
                    break;
            }

            int num = bookmarks.Count();
            for (int i=0;i<num;i++)
            {
                
                var temp = bookmarks[i].Id;
                var comments = db.Comments.Where(m => m.BookmarkId == temp);
                bookmarks[i].CommentsList = new List<Comment>();
                if (comments.Count() > 0)
                {
                    
                    bookmarks[i].CommentsList = comments.ToList<Comment>();
                    ViewBag.hasComments = true;
                }
                var urls = db.SimilarUrls.Where(m => m.BookmarkId == temp);
                bookmarks[i].SimilarUrls = new List<SimilarUrl>();
                if (urls.Count() > 0)
                {

                    bookmarks[i].SimilarUrls = urls.ToList<SimilarUrl>();
                    ViewBag.hasUrl = true;
                }
                var Tags = db.BookmarkTagLinks.Where(c => c.BookmarkId == temp).ToList<BookmarkTagLink>();            
                bookmarks[i].TagsNames = new List<string>();
                bool go = true;
                if (!String.IsNullOrEmpty(SearchString) && !String.IsNullOrEmpty(by))
                    if (string.Compare("tag", by) == 0)
                         go = false;
                foreach (BookmarkTagLink tag in Tags)
                {
                    
                    var TagsFull = db.Tags.Where(c => c.Id == tag.TagId).Select(c => c.Name).ToList<string>();
                    if (!String.IsNullOrEmpty(SearchString))
                    {
                        if (string.Compare(TagsFull[0], SearchString) == 0)
                        {
                            go = true;
                            //System.Diagnostics.Debug.WriteLine("wfdsfdsa");
                        }

                    }
                    bookmarks[i].TagsNames.Add(TagsFull[0]);
                }
                if (go == false && !String.IsNullOrEmpty(SearchString))
                {
                    bookmarks.RemoveAt(i);
                    num--;
                    i--;
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