using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fortest.Context;
using fortest.Models;

namespace fortest.Controllers
{
    public class LoginController : Controller
    {
        RealSetateEntities dbobj = new RealSetateEntities();

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult LoginForm(LoginViewModel model)
        {
            var user = dbobj.Users.FirstOrDefault(x => x.Username.ToLower() == model.Username.ToLower() && x.Password == model.Password);
            if (user != null)
            {
                // Store user role in session
                Session["User"] = user.Role;
                Session["UserId"] = user.UserId;
                Session["Name"] = user.Name;


                if (user.Role==1)
                {
                    return RedirectToAction("Dashboard", "Dashboard");
                }
                else if (user.Role == 2)
                {
                    return RedirectToAction("AgentProfile", "AgentPanel");
                }
                else if (user.Role == 3)
                {
                    return RedirectToAction("UserPageTest", "Login");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid role. Access denied.");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }
        }


        public ActionResult UserPageTest()
        {
            var Name = Session["Name"]?.ToString();
            ViewBag.Username = Name;
            return View();
        }



        public ActionResult Index()
        {
            return View();
        }
    }
}
