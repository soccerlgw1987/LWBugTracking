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
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttachments
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var ticketAttachments = db.TicketAttachments.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketAttachments.ToList());
        }

        // GET: TicketAttachments/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            if (User.Identity.GetUserId() == "db9a774b-807c-4b9b-9b22-34c191872996" || User.Identity.GetUserId() == "3eaa1491-7553-40fa-b7e1-b994e05d05e0" || User.Identity.GetUserId() == "5f84068f-4213-4d02-81a4-21936ae10cdc" || User.Identity.GetUserId() == "60f316c5-536c-4f06-83d3-38a555febc29")
            {
                return RedirectToAction("InvalidAttempt", "Home");
            }

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketId")] TicketAttachment ticketAttachment, string AttachmentDescription, HttpPostedFileBase file)
        {
            var notificationHelper = new NotificationHelper();
            var projHelper = new ProjectHelper();

            if (ModelState.IsValid)
            {
                //if (User.Identity.GetUserId() == "db9a774b-807c-4b9b-9b22-34c191872996")
                //{
                //    if (ticketAttachment.Ticket.AssignedToUserId == "5f84068f-4213-4d02-81a4-21936ae10cdc" || ticketAttachment.Ticket.OwnerUserId == "60f316c5-536c-4f06-83d3-38a555febc29")
                //    {
                //        if (FileUploadValidator.IsWebFriendlyImage(file))
                //        {
                //            var fileName = Path.GetFileName(file.FileName);
                //            file.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                //            ticketAttachment.FilePath = "/Uploads/" + fileName;
                //        }
                //        ticketAttachment.Description = AttachmentDescription;
                //        ticketAttachment.UserId = User.Identity.GetUserId();
                //        ticketAttachment.Created = DateTime.Now;
                //        db.TicketAttachments.Add(ticketAttachment);
                //        db.SaveChanges();

                //        notificationHelper.GetAttachmentNotification(ticketAttachment.TicketId);

                //        return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketId });
                //    }
                //    else
                //    {
                //        return RedirectToAction("InvalidAttempt", "Home");
                //    }
                //}

                if (FileUploadValidator.IsWebFriendlyImage(file))
                {
                    var fileName = Path.GetFileName(file.FileName);
                    file.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    ticketAttachment.FilePath = "/Uploads/" + fileName;
                }
                ticketAttachment.Description = AttachmentDescription;
                ticketAttachment.UserId = User.Identity.GetUserId();
                ticketAttachment.Created = DateTime.Now;
                db.TicketAttachments.Add(ticketAttachment);
                db.SaveChanges();

                notificationHelper.GetAttachmentNotification(ticketAttachment.TicketId);

                return RedirectToAction("Details","Tickets", new { id =ticketAttachment.TicketId});
            }

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Edit/5
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
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,TicketId,FilePath,Description,Created")] TicketAttachment ticketAttachment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttachment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Delete/5
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
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int TicketId)
        {
            var notificationHelper = new NotificationHelper();
            var projHelper = new ProjectHelper();
            var ticket = new Ticket();
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(TicketId);

            //if (User.Identity.GetUserId() == "db9a774b-807c-4b9b-9b22-34c191872996")
            //{
            //    if (ticketAttachment.Ticket.AssignedToUserId == "5f84068f-4213-4d02-81a4-21936ae10cdc" || ticketAttachment.Ticket.OwnerUserId == "60f316c5-536c-4f06-83d3-38a555febc29")
            //    {
            //        db.TicketAttachments.Remove(ticketAttachment);
            //        db.SaveChanges();
            //        return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketId });
            //    }
            //    else
            //    {
            //        return RedirectToAction("InvalidAttempt", "Home");
            //    }
            //}

            db.TicketAttachments.Remove(ticketAttachment);
            db.SaveChanges();
            return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketId });
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
