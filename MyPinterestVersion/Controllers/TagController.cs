using MyPinterestVersion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPinterestVersion.Controllers
{

    [Authorize(Roles = "Administrator")]
    public class TagController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: Category
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            var categories = from category in db.Tags
                             orderby category.Name
                             select category;
            ViewBag.Categories = categories;
            return View();
        }

        public ActionResult Show(int id)
        {
            Tag category = db.Tags.Find(id);
            return View(category);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(Tag cat)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Tags.Add(cat);
                    db.SaveChanges();
                    TempData["message"] = "Tag has been added!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(cat);
                }
            }
            catch (Exception e)
            {
                return View(cat);
            }
        }

        public ActionResult Edit(int id)
        {
            Tag category = db.Tags.Find(id);
            return View(category);
        }

        [HttpPut]
        public ActionResult Edit(int id, Tag requestCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Tag category = db.Tags.Find(id);
                    if (TryUpdateModel(category))
                    {
                        category.Name = requestCategory.Name;
                        TempData["message"] = "Categoria a fost modificata!";
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(requestCategory);
                }
            }
            catch (Exception e)
            {
                return View(requestCategory);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Tag category = db.Tags.Find(id);
            db.Tags.Remove(category);
            TempData["message"] = "Categoria a fost stearsa!";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}