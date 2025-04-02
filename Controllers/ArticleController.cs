using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fortest.Context;
using fortest.Models;

namespace fortest.Controllers
{
    public class ArticleController : Controller
    {
        RealSetateEntities dbobj = new RealSetateEntities();

        public ActionResult AddArticle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddArticle(Article model, HttpPostedFileBase ArtFile)
        {
            if (ModelState.IsValid)
            {
                var newArticles = new Article
                {
                    ArtName = model.ArtName,
                    ArtPostedBy = model.ArtPostedBy,
                    ArtDetails = model.ArtDetails,
                    ArtDate = DateTime.Now
                };

                // Handle file upload
                if (ArtFile != null && ArtFile.ContentLength > 0)
                {
                    // Validate file extension
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(ArtFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("", "Invalid file type. Only .jpg, .jpeg, .png, and .gif are allowed.");
                        return View(model);
                    }

                    // Generate unique file name
                    var uniqueFileName = Guid.NewGuid() + fileExtension;

                    // Define the file path
                    var savePath = Path.Combine(Server.MapPath("~/Content/ArtFile/"), uniqueFileName);

                    // Ensure the directory exists
                    var directory = Path.GetDirectoryName(savePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Save the file to the server
                    ArtFile.SaveAs(savePath);

                    newArticles.ArtFile = "~/Content/ArtFile/" + uniqueFileName;
                }

                // Add and save the new Explore entity to the database
                dbobj.Articles.Add(newArticles);
                dbobj.SaveChanges();

                // Redirect to a success page or the index action
                return RedirectToAction("Index");
            }

            return View();
        }



        public ActionResult PropertyShow()
        {
            var list = dbobj.Explores.ToList();
            return View(list);
        }

        public ActionResult Index()
        {
            
            return View();
        }
    }
}
