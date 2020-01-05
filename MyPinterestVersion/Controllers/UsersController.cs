using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyPinterestVersion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPinterestVersion.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        // GET: Users
        public ActionResult Index()
        {
            var users = from user in db.Users
                        orderby user.UserName
                        select user;
            ViewBag.UsersList = users;
            return View();
        }
        public ActionResult Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            var userRole = user.Roles.FirstOrDefault();
            ViewBag.userRole = userRole.RoleId;
            return View(user);
        }
        [NonAction]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();
            var roles = from role in db.Roles select role;
            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()
                });
            }
            return selectList;
        }
        [HttpPut]
        public ActionResult Edit(string id, ApplicationUser newData)
        {
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            var userRole = user.Roles.FirstOrDefault();
            ViewBag.userRole = userRole.RoleId;
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                if (TryUpdateModel(user))
                {
                    user.UserName = newData.UserName;
                    user.Email = newData.Email;
                    user.PhoneNumber = newData.PhoneNumber;
                    var roles = from role in db.Roles select role;
                    foreach (var role in roles)
                    {
                        UserManager.RemoveFromRole(id, role.Name);
                    }
                    var selectedRole = db.Roles.Find(HttpContext.Request.Params.Get("newRole"));
                    UserManager.AddToRole(id, selectedRole.Name);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Response.Write(e.Message);
                return View(user);
            }
        }
        public ActionResult Show(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            ViewBag.utilizatorCurent = User.Identity.GetUserId();


            var roles = db.Roles.ToList();

            var roleName = roles.Where(j => j.Id ==
               user.Roles.FirstOrDefault().RoleId).
               Select(a => a.Name).FirstOrDefault();

            ViewBag.roleName = roleName;

         
                var currentUserId = id;
            var bookCat = db.CategoryUserBookmarkLinks.Where(m => m.UserId == currentUserId).ToList<CategoryUserBookmarkLink>();
            List<Bookmark> bookmarks = new List<Bookmark>();
            foreach (CategoryUserBookmarkLink p in bookCat)
            {
                var iD = p.BookmarkId;
                var catId = p.CategoryId;
                var cat = db.Categories.Where(m => m.Id == catId).Select(m => m.Name).ToList<string>();

                var bookmark = db.Bookmarks.Include("Image").SingleOrDefault(x => x.Id == iD);
                var comments = db.Comments.Where(m => m.BookmarkId == iD);
                bookmark.CategoryName = cat[0];
                bookmark.CommentsList = new List<Comment>();
                if (comments.Count() > 0)
                {

                    bookmark.CommentsList = comments.ToList<Comment>();
                    ViewBag.hasComments = true;
                }
                var urls = db.SimilarUrls.Where(m => m.BookmarkId == iD);
                bookmark.SimilarUrls = new List<SimilarUrl>();
                if (urls.Count() > 0)
                {

                    bookmark.SimilarUrls = urls.ToList<SimilarUrl>();
                    ViewBag.hasUrl = true;
                }
                if (TempData.ContainsKey("message"))
                {
                    ViewBag.message = TempData["message"].ToString();
                }



                var temp = iD;
                var Tags = db.BookmarkTagLinks.Where(c => c.BookmarkId == temp).ToList<BookmarkTagLink>();

                bookmark.TagsNames = new List<string>();

                foreach (BookmarkTagLink tag in Tags)
                {

                    var TagsFull = db.Tags.Where(c => c.Id == tag.TagId).Select(c => c.Name).ToList<string>();

                    bookmark.TagsNames.Add(TagsFull[0]);
                }
                bookmarks.Add(bookmark);
            }
                user.UserBookmarks = bookmarks;
            
              
            

            return View(user);
        }
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = UserManager.Users.FirstOrDefault(u => u.Id == id);

            var articles = db.Bookmarks.Where(a => a.UserId == id);
            foreach (var article in articles)
            {
                db.Bookmarks.Remove(article);

            }           
            db.SaveChanges();
            UserManager.Delete(user);
            return RedirectToAction("Index");
            
        }
    }
}
