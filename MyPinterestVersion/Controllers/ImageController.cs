using System;
using System.Collections.Generic;
using MyPinterestVersion.Models;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPinterestVersion.Controllers
{
    public class ImageController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Image imageModel)
        {
            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.ImagePath = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);
            db.Images.Add(imageModel);
            db.SaveChanges();

            return RedirectToAction("../Bookmark/New", new { id = imageModel.ImageID });
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            Image imageModel = new Image();
            imageModel = db.Images.Where(x => x.ImageID == id).FirstOrDefault();
            return View(imageModel);
        }
    }
}