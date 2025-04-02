using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fortest.Context;

namespace fortest.Controllers
{
    public class AddUserController : Controller
    {
        RealSetateEntities dbobj = new RealSetateEntities();

        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(User model)
        {
            
            User newUser = new User();

            newUser.Name = model.Name;
            newUser.Username = model.Username;
            newUser.Password = model.Password;
            newUser.isActive = true;
            newUser.isDeleted = false;
            newUser.Role = model.Role;
            newUser.CreatedOn = DateTime.Now;

            dbobj.Users.Add(newUser);
            dbobj.SaveChanges();

            return RedirectToAction("Index");
        }



        public ActionResult Index()
        {
            
            return View();
        }
    }
}
