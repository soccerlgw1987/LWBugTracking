using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projHelper = new ProjectHelper();

        // GET: Projects
        [Authorize(Roles = "Admin,Project Manager")]
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        [Authorize]
        public ActionResult IndexMy()
        {
            var myProjects = projHelper.ListUserProjects(User.Identity.GetUserId());

            return View(myProjects);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin,Project Manager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.ProjectStatusId = db.ProjectStatuses.FirstOrDefault(p => p.Status == "New").Id;
                project.Created = DateTime.Now;
                project.CompletionDate = project.Created.AddDays(7);
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin,Project Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            //var projectManager = UserRolesHelper.UsersInRole("Project Manager");
            //ViewBag.ProjectManager = new SelectList(projectManager, "Id", "DisplayName");
            var projUsers = projHelper.UsersOnProject(id ?? 0);

            string assignedPM = "";
            string assignedSub = "";
            List<string> assignedDevs = new List<string>();

            var pms = roleHelper.UsersInRole("Project Manager");
            foreach (var user in projUsers)
            {
                if (roleHelper.IsUserInRole(user.Id, "Project Manager"))
                    assignedPM = user.Id;
            }
            ViewBag.ProjectManager = new SelectList(pms, "Id", "Email", assignedPM);

            var subs = roleHelper.UsersInRole("Submitter");
            foreach (var user in projUsers)
            {
                if (roleHelper.IsUserInRole(user.Id, "Submitter"))
                    assignedSub = user.Id;
            }
            ViewBag.Submitter = new SelectList(subs, "Id", "Email", assignedSub);

            var devs = roleHelper.UsersInRole("Developer");
            foreach (var user in projUsers)
            {
                if (roleHelper.IsUserInRole(user.Id, "Developer"))
                    assignedDevs.Add(user.Id);
            }
            ViewBag.Developers = new MultiSelectList(devs, "Id", "Email", assignedDevs);


            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,CompletionDate,Created,ProjectStatusId")] Project project, string ProjectManager, string Submitter, List<string> Developers)
        {
            //List all users on this project
            var projUsers = new List<ApplicationUser>();

            if (ModelState.IsValid)
            {

                projUsers = projHelper.UsersOnProject(project.Id).ToList();
                //Remove all users from this project
                foreach (var user in projUsers.ToList())
                {
                    projHelper.RemoveUserFromProject(user.Id, project.Id);
                }

                projHelper.AddUserToProject(ProjectManager, project.Id);
                projHelper.AddUserToProject(Submitter, project.Id);

                if (Developers != null)
                {
                    foreach (var dev in Developers)
                    {
                        projHelper.AddUserToProject(dev, project.Id);
                    }
                }

                var currentStatus = db.ProjectStatuses.FirstOrDefault(p => p.Id == project.ProjectStatusId).Status;
                if(currentStatus == "New")
                {
                    project.ProjectStatusId = db.ProjectStatuses.FirstOrDefault(p => p.Status == "In Progress").Id;
                }
                
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            string assignedPM = "";
            string assignedSub = "";
            List<string> assignedDevs = new List<string>();

            var pms = roleHelper.UsersInRole("Project Manager");
            foreach (var user in projUsers)
            {
                if (roleHelper.IsUserInRole(user.Id, "Project Manager"))
                    assignedPM = user.Id;
            }
            ViewBag.ProjectManager = new SelectList(pms, "Id", "Email", assignedPM);

            var subs = roleHelper.UsersInRole("Submitter");
            foreach (var user in projUsers)
            {
                if (roleHelper.IsUserInRole(user.Id, "Submitter"))
                    assignedSub = user.Id;
            }
            ViewBag.Submitter = new SelectList(subs, "Id", "Email", assignedSub);

            var devs = roleHelper.UsersInRole("Developer");
            foreach (var user in projUsers)
            {
                if (roleHelper.IsUserInRole(user.Id, "Developer"))
                    assignedDevs.Add(user.Id);
            }
            ViewBag.Developers = new MultiSelectList(devs, "Id", "Email", assignedDevs);


            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin,Project Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
    }
}
