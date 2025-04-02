using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fortest.Context;

namespace fortest.Controllers
{
    public class ReviewsController : Controller
    {
        RealSetateEntities dbobj = new RealSetateEntities();

        
        public ActionResult UploadReview()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadReviews(ClientsReview model, HttpPostedFileBase RevFile)
        {
            if (ModelState.IsValid)
            {
                // Create a new Explore object
                var newClientsReview = new ClientsReview
                {
                    RevName = model.RevName,
                    RevLocation = model.RevLocation,
                    RevRating = model.RevRating,
                    RevReviewDetail = model.RevReviewDetail,
                    isCreated = DateTime.Now
                };

                // Handle file upload
                if (RevFile != null && RevFile.ContentLength > 0)
                {
                    // Validate file extension
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(RevFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("", "Invalid file type. Only .jpg, .jpeg, .png, and .gif are allowed.");
                        return View(model);
                    }

                    // Generate unique file name
                    var uniqueFileName = Guid.NewGuid() + fileExtension;

                    // Define the file path
                    var savePath = Path.Combine(Server.MapPath("~/Content/RevFile/"), uniqueFileName);

                    // Ensure the directory exists
                    var directory = Path.GetDirectoryName(savePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Save the file to the server
                    RevFile.SaveAs(savePath);

                    // Save the file path in the database
                    newClientsReview.RevFile = "~/Content/RevFile/" + uniqueFileName;
                }

                // Add and save the new Explore entity to the database
                dbobj.ClientsReviews.Add(newClientsReview);
                dbobj.SaveChanges();

                // Redirect to a success page or the index action
                return RedirectToAction("Index");
            }

            // Return view with validation errors
            return View(model);
        }



        public ActionResult ReviewsShow()
        {
            var list = dbobj.ClientsReviews.ToList();
            return View(list);
        }

        public ActionResult Index()
        {
            
            return View();
        }
    }
}
