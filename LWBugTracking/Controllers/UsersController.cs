using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LWBugTracking.Helper;
using LWBugTracking.Models;
using Microsoft.AspNet.Identity;

namespace LWBugTracking.Controllers
{
    [RequireHttps]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        // GET: Users
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: Users/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            if (User.Identity.GetUserId() == "db9a774b-807c-4b9b-9b22-34c191872996" || User.Identity.GetUserId() == "3eaa1491-7553-40fa-b7e1-b994e05d05e0" || User.Identity.GetUserId() == "5f84068f-4213-4d02-81a4-21936ae10cdc" || User.Identity.GetUserId() == "60f316c5-536c-4f06-83d3-38a555febc29")
            {
                return RedirectToAction("InvalidAttempt", "Home");
            }

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,DisplayName,AvatarPath,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(applicationUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            //allow a role to be selected
            var currentRole = roleHelper.ListUserRoles(id).FirstOrDefault();
            ViewBag.Roles = new SelectList(db.Roles, "Name", "Name", currentRole);

            return View(applicationUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,DisplayName,AvatarPath,Email,SecurityStamp,PhoneNumber,PasswordHash,UserName")] ApplicationUser applicationUser, string roles)
        {
            if (User.Identity.GetUserId() == "db9a774b-807c-4b9b-9b22-34c191872996" || User.Identity.GetUserId() == "3eaa1491-7553-40fa-b7e1-b994e05d05e0" || User.Identity.GetUserId() == "5f84068f-4213-4d02-81a4-21936ae10cdc" || User.Identity.GetUserId() == "60f316c5-536c-4f06-83d3-38a555febc29")
            {
                return RedirectToAction("InvalidAttempt", "Home");
            }

            if (ModelState.IsValid)
            {
                //since i might be changing the users role, i want to first remove from all roles
                var currentRoles = roleHelper.ListUserRoles(applicationUser.Id);
                foreach(var role in currentRoles)
                {
                    roleHelper.RemoveUserFromRole(applicationUser.Id, role);
                }
                if(!string.IsNullOrEmpty(roles))
                    roleHelper.AddUserToRole(applicationUser.Id, roles);

                applicationUser.UserName = applicationUser.Email;

                db.Entry(applicationUser).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: Users/Edit/5
        public ActionResult EditMy(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            //allow a role to be selected
            var currentRole = roleHelper.ListUserRoles(id).FirstOrDefault();
            ViewBag.Roles = new SelectList(db.Roles, "Name", "Name", currentRole);

            return View(applicationUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMy([Bind(Include = "Id,FirstName,LastName,DisplayName,AvatarPath,Email,SecurityStamp,PhoneNumber,PasswordHash,UserName")] ApplicationUser applicationUser, string roles, HttpPostedFileBase image)
        {
            if (User.Identity.GetUserId() == "db9a774b-807c-4b9b-9b22-34c191872996" || User.Identity.GetUserId() == "3eaa1491-7553-40fa-b7e1-b994e05d05e0" || User.Identity.GetUserId() == "5f84068f-4213-4d02-81a4-21936ae10cdc" || User.Identity.GetUserId() == "60f316c5-536c-4f06-83d3-38a555febc29")
            {
                return RedirectToAction("InvalidAttempt", "Home");
            }

            if (ModelState.IsValid)
            {
                //since i might be changing the users role, i want to first remove from all roles
                var currentRoles = roleHelper.ListUserRoles(applicationUser.Id);
                foreach (var role in currentRoles)
                {
                    roleHelper.RemoveUserFromRole(applicationUser.Id, role);
                }
                if (!string.IsNullOrEmpty(roles))
                    roleHelper.AddUserToRole(applicationUser.Id, roles);

                if (FileUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    applicationUser.AvatarPath = "/Uploads/" + fileName;
                }

                applicationUser.UserName = applicationUser.Email;

                db.Entry(applicationUser).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Profile");
            }
            return View(applicationUser);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (User.Identity.GetUserId() == "db9a774b-807c-4b9b-9b22-34c191872996" || User.Identity.GetUserId() == "3eaa1491-7553-40fa-b7e1-b994e05d05e0" || User.Identity.GetUserId() == "5f84068f-4213-4d02-81a4-21936ae10cdc" || User.Identity.GetUserId() == "60f316c5-536c-4f06-83d3-38a555febc29")
            {
                return RedirectToAction("InvalidAttempt", "Home");
            }

            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public new ActionResult Profile()
        {
            return View();
        }

    }
}
