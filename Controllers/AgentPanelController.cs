using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fortest.Context;
using PagedList;
using fortest.Models;
using System.Drawing.Printing;
using System.Web.UI;

namespace fortest.Controllers
{
    public class AgentPanelController : Controller
    {
        RealSetateEntities dbobj = new RealSetateEntities();

        public ActionResult AgentPropertyPage(int page = 1, int pageSize = 2)
        {

            int? userId = Session["UserId"] as int?;
            var listcount = dbobj.Explores.Count(x => x.UserId == userId);
            ViewBag.listcount = listcount;

            var list = dbobj.Explores
                .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.isCreated)
                .Skip((page - 1) * pageSize)
            .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)listcount / pageSize);

            return View(list);
        }


        public ActionResult AgentAddProperty()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgentAddProperty(Explore model, HttpPostedFileBase ExpFile)
        {
            if (ModelState.IsValid)
            {
                int? userId = Session["UserId"] as int?;

                var newExplore = new Explore
                {
                    ExpName = model.ExpName,
                    ExpPrice = model.ExpPrice,
                    ExpType = model.ExpType,
                    ExpRating=model.ExpRating,
                    ExpDescription = model.ExpDescription,
                    Purpose=model.Purpose,
                    isCreated = DateTime.Now,
                    UserId=userId
                };

                // Handle file upload
                if (ExpFile != null && ExpFile.ContentLength > 0)
                {
                    // Validate file extension
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(ExpFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("", "Invalid file type. Only .jpg, .jpeg, .png, and .gif are allowed.");
                        return View(model);
                    }

                    // Generate unique file name
                    var uniqueFileName = Guid.NewGuid() + fileExtension;

                    // Define the file path
                    var savePath = Path.Combine(Server.MapPath("~/Content/Explore/"), uniqueFileName);

                    // Ensure the directory exists
                    var directory = Path.GetDirectoryName(savePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Save the file to the server
                    ExpFile.SaveAs(savePath);

                    // Save the file path in the database
                    newExplore.ExpFile = "~/Content/Explore/" + uniqueFileName;
                }

                dbobj.Explores.Add(newExplore);
                dbobj.SaveChanges();

                return RedirectToAction("");
            }

            return View(model);
        }

        public ActionResult AgentDeleteProperty(int id)
        {
            var delete = dbobj.Explores.FirstOrDefault(x => x.UserId == id);
            if (delete != null)
            {
                dbobj.Explores.Remove(delete);
                dbobj.SaveChanges();
            }
            int? userid = Session["UserId"] as int?;
            var list = dbobj.Explores.Where(x => x.UserId == userid);
            return View("AgentPropertyPage", list);
        }

        public ActionResult AgentProfile()
        {
            int? userId = Session["UserId"] as int?;

            var list = dbobj.Users.Where(x => x.UserId == userId).ToList();

            return View(list);
        }

        public ActionResult SearchType(string category)
        {
            int? userid = Session["UserId"] as int?;
            var results = dbobj.Explores.Where(x => x.UserId == userid)
                .Where(e => e.ExpType.Contains(category))
                .Select(e => new
                {
                    e.ExpFile,
                    e.ExpName,
                    e.ExpRating,
                    e.ExpPrice,
                    e.ExpType,
                    e.ExpDescription,
                    e.isCreated
                })
                .ToList();
            return Json(new { success = true, properties = results }, JsonRequestBehavior.AllowGet);
        }




        //public ActionResult AgentPropertyPage(int page = 1, int pageSize = 5)
        //{
        //    int? userId = Session["UserId"] as int?;
        //    var listcount = dbobj.Explores.Count(x => x.UserId == userId);
        //    ViewBag.listcount = listcount;

        //    var list = dbobj.Explores
        //        .Where(x => x.UserId == userId)
        //        .OrderByDescending(x => x.isCreated)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //    ViewBag.CurrentPage = page;
        //    ViewBag.TotalPages = (int)Math.Ceiling((double)listcount / pageSize);

        //    return View(list);
        //}


    }
}
