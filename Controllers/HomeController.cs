using fortest.Context;
using fortest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace fortest.Controllers
{
    public class HomeController : Controller
    {
        private readonly RealSetateEntities dbobj = new RealSetateEntities();
        public ActionResult Index()
        {
            using (var db = new RealSetateEntities())
            {
                var exploreDetails = db.Explores.OrderByDescending(e => e.ExploreId)
                                       .Take(6)
                                       .ToList();
                var articleDetails = db.Articles.ToList();
                var reviewDetails = db.ClientsReviews.ToList();
                var HowWorkDetails = db.HowWorks.ToList();

                var viewModel = new AllViewModel
                {
                    ExplorModels = exploreDetails ?? new List<Explore>(),
                    ArticleModels = articleDetails ?? new List<Article>(),
                    ReviewModels=reviewDetails ?? new List<ClientsReview>(),
                    HowWorkModels= HowWorkDetails ?? new List<HowWork>()
                };

                //counter start
                int totalProperties = dbobj.Explores.Count();

                int restaurantCount = dbobj.Explores.Count(p => p.ExpType == "Restaurant");
                int houseCount = dbobj.Explores.Count(p => p.ExpType == "House");
                int officeCount = dbobj.Explores.Count(p => p.ExpType == "Office");
                int hotelsCount = dbobj.Explores.Count(p => p.ExpType == "Hotel");

                ViewBag.TotalProperties = totalProperties;
                ViewBag.RestaurantCount = restaurantCount;
                ViewBag.HouseCount = houseCount;
                ViewBag.OfficeCount = officeCount;
                ViewBag.HotelsCount = hotelsCount;
                //counter end


                return View(viewModel); // Pass a single AllViewModel instance
            }
        }


        [HttpPost]
        public JsonResult AddAccount(AddBusiness model)
        {
            if (ModelState.IsValid)
            {
                AddBusiness business = new AddBusiness
                {
                    BusiEmail = model.BusiEmail,
                    isCreated = DateTime.Now
                };

                dbobj.AddBusinesses.Add(business);
                dbobj.SaveChanges();
                return Json(new { success = true, message = "Business account added successfully!" });
            }
            return Json(new { success = false, message = "Error while adding business account." });
        }



        public ActionResult HowItWorks(int Id)
        {
            var data = dbobj.HowWorks.FirstOrDefault(x => x.Id == Id);
            return View(data);
        }

        public ActionResult PropertyDetail(int ExploreId)
        {
            var data = dbobj.Explores.FirstOrDefault(x => x.ExploreId == ExploreId);
            return View(data);
        }


        [HttpGet]
        public JsonResult SearchByName(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return Json(new { success = false, message = "Search term cannot be empty." }, JsonRequestBehavior.AllowGet);
            }

            var matchingProperties = dbobj.Explores
                                           .Where(e => e.ExpType.Contains(searchTerm) || e.ExpDescription.Contains(searchTerm))
                                           .Select(e => new
                                           {
                                               e.ExpFile,
                                               e.ExploreId,
                                               e.ExpName,
                                               e.ExpType,
                                               e.ExpDescription
                                           })
                                           .ToList();

            return Json(new { success = true, properties = matchingProperties }, JsonRequestBehavior.AllowGet);
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