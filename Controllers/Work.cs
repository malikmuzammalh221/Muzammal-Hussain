using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fortest.Context;
using fortest.Models;

namespace fortest.Controllers
{
    public class WorkController : Controller
    {
        RealSetateEntities dbobj = new RealSetateEntities();

        public ActionResult Work()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Works(HowWork model)
        {
            if (ModelState.IsValid)
            {
                var Works = new HowWork
                {
                    Name = model.Name,
                    Description = model.Description,
                    Icon=model.Icon
                };

                dbobj.HowWorks.Add(Works);
                dbobj.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Index()
        {
            
            return View();
        }
    }
}
