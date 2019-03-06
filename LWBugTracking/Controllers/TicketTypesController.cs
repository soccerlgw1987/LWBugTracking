using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LWBugTracking.Models;
using Microsoft.AspNet.Identity;

namespace LWBugTracking.Controllers
{
    [RequireHttps]
    [Authorize]
    public class TicketTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketTypes
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.TicketTypes.ToList());
        }

        // GET: TicketTypes/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketType ticketType = db.TicketTypes.Find(id);
            if (ticketType == null)
            {
                return HttpNotFound();
            }
            return View(ticketType);
        }

        // GET: TicketTypes/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            if (User.Identity.GetUserId() == "db9a774b-807c-4b9b-9b22-34c191872996" || User.Identity.GetUserId() == "3eaa1491-7553-40fa-b7e1-b994e05d05e0" || User.Identity.GetUserId() == "5f84068f-4213-4d02-81a4-21936ae10cdc" || User.Identity.GetUserId() == "60f316c5-536c-4f06-83d3-38a555febc29")
            {
                return RedirectToAction("InvalidAttempt", "Home");
            }

            return View();
        }

        // POST: TicketTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] TicketType ticketType)
        {
            if (ModelState.IsValid)
            {
                db.TicketTypes.Add(ticketType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ticketType);
        }

        // GET: TicketTypes/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (User.Identity.GetUserId() == "db9a774b-807c-4b9b-9b22-34c191872996" || User.Identity.GetUserId() == "3eaa1491-7553-40fa-b7e1-b994e05d05e0" || User.Identity.GetUserId() == "5f84068f-4213-4d02-81a4-21936ae10cdc" || User.Identity.GetUserId() == "60f316c5-536c-4f06-83d3-38a555febc29")
            {
                return RedirectToAction("InvalidAttempt", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketType ticketType = db.TicketTypes.Find(id);
            if (ticketType == null)
            {
                return HttpNotFound();
            }
            return View(ticketType);
        }

        // POST: TicketTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] TicketType ticketType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticketType);
        }

        // GET: TicketTypes/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (User.Identity.GetUserId() == "db9a774b-807c-4b9b-9b22-34c191872996" || User.Identity.GetUserId() == "3eaa1491-7553-40fa-b7e1-b994e05d05e0" || User.Identity.GetUserId() == "5f84068f-4213-4d02-81a4-21936ae10cdc" || User.Identity.GetUserId() == "60f316c5-536c-4f06-83d3-38a555febc29")
            {
                return RedirectToAction("InvalidAttempt", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketType ticketType = db.TicketTypes.Find(id);
            if (ticketType == null)
            {
                return HttpNotFound();
            }
            return View(ticketType);
        }

        // POST: TicketTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketType ticketType = db.TicketTypes.Find(id);
            db.TicketTypes.Remove(ticketType);
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
