using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using fortest.Context;
using fortest.Models;

namespace fortest.Controllers
{
    public class DashboardController : Controller
    {
        RealSetateEntities dbobj = new RealSetateEntities();

        
        public ActionResult Dashboard()
        {
            //list count
            var PropertyList = dbobj.Explores.Count();
            var restaurantList = dbobj.Explores.Count(e => e.ExpType == "Restaurant");
            var hotelList = dbobj.Explores.Count(e => e.ExpType == "Hotel");
            var houseList = dbobj.Explores.Count(e => e.ExpType == "House");
            var officeList = dbobj.Explores.Count(e => e.ExpType == "Office");
            ViewBag.PropertyList = PropertyList;
            ViewBag.RestaurantList = restaurantList;
            ViewBag.HotelList = hotelList;
            ViewBag.HouseList = houseList;
            ViewBag.OfficeList = officeList;
            return View();
        }


        public ActionResult PropertyPage(int Page=1,int PageSize=5)
        {
            
            var exploreDetails = dbobj.Explores.ToList();
            var articleDetails = dbobj.Articles.ToList();
            var reviewDetails = dbobj.ClientsReviews.ToList();
            var HowWorkDetails = dbobj.HowWorks.ToList();

            var viewModel = new DashboardViewModel
            {
                ExplorModels = exploreDetails ?? new List<Explore>(),
                ArticleModels = articleDetails ?? new List<Article>(),
                ReviewModels = reviewDetails ?? new List<ClientsReview>(),
                HowWorkModels = HowWorkDetails ?? new List<HowWork>()
            };

            //list count
            var PropertyList = dbobj.Explores.Count();
            ViewBag.PropertyList = PropertyList;
            var exploreDetail = dbobj.Explores.OrderByDescending(x => x.isCreated)
                                        .Skip((Page - 1) * PageSize)
                                        .Take(PageSize)
                                        .ToList();
            ViewBag.CurrentPage = Page;
            ViewBag.TotalPage = (int)Math.Ceiling((double)PropertyList / PageSize);
            viewModel.ExplorModels = exploreDetail;
            return View(viewModel);
        }

        public ActionResult DeleteProperty(int ExploreId)
        {
            var delete = dbobj.Explores.FirstOrDefault(x => x.ExploreId == ExploreId);
            if (delete != null)
            {
                dbobj.Explores.Remove(delete);
                dbobj.SaveChanges();
            }
            return RedirectToAction("PropertyPage");
        }

        [HttpGet]
        public JsonResult SearchProperty(string category)
        {
            var results = dbobj.Explores
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


        public ActionResult ArticlePage()
        {
            var exploreDetails = dbobj.Explores.ToList();
            var articleDetails = dbobj.Articles.ToList();
            var reviewDetails = dbobj.ClientsReviews.ToList();
            var HowWorkDetails = dbobj.HowWorks.ToList();

            var viewModel = new DashboardViewModel
            {
                ExplorModels = exploreDetails ?? new List<Explore>(),
                ArticleModels = articleDetails ?? new List<Article>(),
                ReviewModels = reviewDetails ?? new List<ClientsReview>(),
                HowWorkModels = HowWorkDetails ?? new List<HowWork>()
            };
            var Articlelist = dbobj.Articles.Count();
            ViewBag.ArticleList = Articlelist;
            return View(viewModel);
        }

        public ActionResult ReviewPage()
        {
            var exploreDetails = dbobj.Explores.ToList();
            var articleDetails = dbobj.Articles.ToList();
            var reviewDetails = dbobj.ClientsReviews.ToList();
            var HowWorkDetails = dbobj.HowWorks.ToList();

            var viewModel = new DashboardViewModel
            {
                ExplorModels = exploreDetails ?? new List<Explore>(),
                ArticleModels = articleDetails ?? new List<Article>(),
                ReviewModels = reviewDetails ?? new List<ClientsReview>(),
                HowWorkModels = HowWorkDetails ?? new List<HowWork>()
            };
            return View(viewModel);
        }

        public ActionResult WorkPage()
        {
            var exploreDetails = dbobj.Explores.ToList();
            var articleDetails = dbobj.Articles.ToList();
            var reviewDetails = dbobj.ClientsReviews.ToList();
            var HowWorkDetails = dbobj.HowWorks.ToList();

            var viewModel = new DashboardViewModel
            {
                ExplorModels = exploreDetails ?? new List<Explore>(),
                ArticleModels = articleDetails ?? new List<Article>(),
                ReviewModels = reviewDetails ?? new List<ClientsReview>(),
                HowWorkModels = HowWorkDetails ?? new List<HowWork>()
            };
            return View(viewModel);
        }

        public ActionResult Pagination(int page = 1, int pageSize = 5)
        {
            var properties = dbobj.Explores.OrderBy(p => p.isCreated).ToList(); // Order by date or any other field

            int totalRecords = properties.Count();
            var paginatedProperties = properties.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return View(paginatedProperties);
        }

        public ActionResult Conta()
        {
            return View();
        }
    }
}
