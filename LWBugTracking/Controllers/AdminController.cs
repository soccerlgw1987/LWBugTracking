using LWBugTracking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LWBugTracking.Controllers
{
    [RequireHttps]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult AssignRole()
        {
            //Holds users
            ViewBag.Users1 = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.Users2 = new SelectList(db.Users, "Id", "Email");

            //Holds roles
            ViewBag.Roles1 = new SelectList(db.Roles, "Id", "Name");
            ViewBag.Roles2 = new SelectList(db.Roles, "Name", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignRole(string users1, string users2, string roles1, string roles2)
        {
            return RedirectToAction("Index", "Home");
        }

    }
}