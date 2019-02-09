﻿using System;
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
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projHelper = new ProjectHelper();

        // GET: Tickets
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());
        }

        // GET: Tickets/Details/5/Attachments
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //var ticket = db.Tickets.AsNoTracking().Include(t => t.TicketAttachments).Where(t => t.Id == id).FirstOrDefault();
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            if (ticket.AssignedToUserId == User.Identity.GetUserId() || ticket.OwnerUser.Email == User.Identity.Name || User.IsInRole("Admin"))
            {
                return View(ticket);
            }

            return RedirectToAction("InvalidAttempt", "Home");
        }

        // GET: Tickets/Details/5/Comments
        public ActionResult DetailsComments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //var ticket = db.Tickets.AsNoTracking().Include(t => t.TicketAttachments).Where(t => t.Id == id).FirstOrDefault();
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            if (ticket.AssignedToUserId == User.Identity.GetUserId() || ticket.OwnerUser.Email == User.Identity.Name || User.IsInRole("Admin"))
            {
                return View(ticket);
            }

            return RedirectToAction("InvalidAttempt", "Home");
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create()
        {
            
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Description,ProjectId,TicketTypeId,TicketPriorityId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "New").Id;
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.Created = DateTime.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            var roleHelper = new UserRolesHelper();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            if (ticket.AssignedToUserId == User.Identity.GetUserId() || ticket.OwnerUser.Email == User.Identity.Name || User.IsInRole("Admin"))
            {
                ViewBag.AssignedToUser = new SelectList(roleHelper.UsersInRole("Developer"), "Id", "FirstName", ticket.AssignedToUser);
                ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
                ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
                ViewBag.TicketStatusId = new SelectList(db.TicketStatuses.Where(s => s.Name != "New"), "Id", "Name", ticket.TicketStatusId);
                ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
                return View(ticket);
            }
            return RedirectToAction("InvalidAttempt","Home");
            
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var currentStatus = db.TicketStatuses.FirstOrDefault(t => t.Id == ticket.TicketStatusId).Name;
                if (currentStatus == "New")
                {
                    ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "In Progress").Id;
                }

                ticket.Updated = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
