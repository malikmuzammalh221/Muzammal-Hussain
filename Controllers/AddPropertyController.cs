using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fortest.Context;

namespace fortest.Controllers
{
    public class AddPropertyController : Controller
    {
        RealSetateEntities dbobj = new RealSetateEntities();

        [HttpGet]
        public ActionResult AddProperty()
        {
            return View(new Explore());
        }

        [HttpPost]
        public ActionResult AddPropertys(Explore model, HttpPostedFileBase ExpFile)
        {
            if (ModelState.IsValid)
            {
                int? userid = Session["UserId"] as int?;
                // Create a new Explore object
                var newExplore = new Explore
                {
                    ExpName = model.ExpName,
                    ExpPrice = model.ExpPrice,
                    ExpType = model.ExpType,
                    ExpDescription = model.ExpDescription,
                    ExpRating=model.ExpRating,
                    Purpose=model.Purpose,
                    isCreated = DateTime.Now,
                    UserId=userid
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

                // Add and save the new Explore entity to the database
                dbobj.Explores.Add(newExplore);
                dbobj.SaveChanges();

                // Redirect to a success page or the index action
                return RedirectToAction("Index");
            }

            // Return view with validation errors
            return View(model);
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
