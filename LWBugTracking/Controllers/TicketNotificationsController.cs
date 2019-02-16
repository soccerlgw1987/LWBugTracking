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
    public class TicketNotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketNotifications
        public ActionResult Index()
        {
            var ticketNotifications = db.TicketNotifications.Include(t => t.Recipient).Include(t => t.Ticket);
            return View(ticketNotifications.ToList());
        }

        // GET: TicketNotifications/Details/5
        public ActionResult Details(int? id)
        {
            if (User.Identity.GetUserId() == "db9a774b-807c-4b9b-9b22-34c191872996" || User.Identity.GetUserId() == "3eaa1491-7553-40fa-b7e1-b994e05d05e0" || User.Identity.GetUserId() == "5f84068f-4213-4d02-81a4-21936ae10cdc" || User.Identity.GetUserId() == "60f316c5-536c-4f06-83d3-38a555febc29")
            {
                return RedirectToAction("InvalidAttempt", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotification ticketNotification = db.TicketNotifications.Find(id);
            if (ticketNotification == null)
            {
                return HttpNotFound();
            }
            return View(ticketNotification);
        }

        // GET: TicketNotifications/Create
        public ActionResult Create()
        {
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            return View();
        }

        // POST: TicketNotifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,RecipientId,NotificationBody,Created,Read")] TicketNotification ticketNotification)
        {
            if (ModelState.IsValid)
            {
                db.TicketNotifications.Add(ticketNotification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", ticketNotification.RecipientId);
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketNotification.TicketId);
            return View(ticketNotification);
        }

        // GET: TicketNotifications/Edit/5
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
            TicketNotification ticketNotification = db.TicketNotifications.Find(id);
            if (ticketNotification == null)
            {
                return HttpNotFound();
            }
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", ticketNotification.RecipientId);
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketNotification.TicketId);
            return View(ticketNotification);
        }

        // POST: TicketNotifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,RecipientId,NotificationBody,Created,Read")] TicketNotification ticketNotification, bool notificationRead)
        {
            if (ModelState.IsValid)
            {
                ticketNotification.Read = notificationRead;
                db.Entry(ticketNotification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", ticketNotification.RecipientId);
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketNotification.TicketId);
            return View(ticketNotification);
        }

        // GET: TicketNotifications/Delete/5
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
            TicketNotification ticketNotification = db.TicketNotifications.Find(id);
            if (ticketNotification == null)
            {
                return HttpNotFound();
            }
            return View(ticketNotification);
        }

        // POST: TicketNotifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketNotification ticketNotification = db.TicketNotifications.Find(id);
            db.TicketNotifications.Remove(ticketNotification);
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
