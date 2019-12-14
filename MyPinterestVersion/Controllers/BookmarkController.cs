using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPinterestVersion.Controllers
{
    public class BookmarkController : Controller
    {
        // GET: Bookmark
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult Add()
        //{
        //    return RedirectToAction();
        //}
        //public ActionResult FileUpload(HttpPostedFileBase file)
        //{

        //    if (file != null)
        //    {
                
        //        string ImageName = System.IO.Path.GetFileName(file.FileName);
        //        string physicalPath = Server.MapPath("~/images/" + ImageName);

        //        // save image in folder
        //        file.SaveAs(physicalPath);

        //    }
        //    //Display records
        //    return RedirectToAction("../home/");
        //}
    }
}