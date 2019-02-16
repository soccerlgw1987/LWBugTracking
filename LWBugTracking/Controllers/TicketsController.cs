using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        private Project projectCreate = new Project();

        // GET: Tickets
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());
        }

        // GET: TicketsHistory
        public ActionResult History(int? id)
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

            if (ticket.AssignedToUserId == User.Identity.GetUserId() || ticket.OwnerUser.Email == User.Identity.Name || User.IsInRole("Admin") || projHelper.IsUserOnProject(User.Identity.GetUserId(), ticket.ProjectId))
            {
                return View(ticket);
            }

            return RedirectToAction("InvalidAttempt", "Home");
        }

        // GET: TicketsDashboard
        public ActionResult TicketsDashboard(int? id)
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

            if (ticket.AssignedToUserId == User.Identity.GetUserId() || ticket.OwnerUser.Email == User.Identity.Name || User.IsInRole("Admin") || projHelper.IsUserOnProject(User.Identity.GetUserId(), ticket.ProjectId))
            {
                return View(ticket);
            }

            return RedirectToAction("InvalidAttempt", "Home");
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

            if (ticket.AssignedToUserId == User.Identity.GetUserId() || ticket.OwnerUser.Email == User.Identity.Name || User.IsInRole("Admin")  || projHelper.IsUserOnProject(User.Identity.GetUserId(),ticket.ProjectId))
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

            if (ticket.AssignedToUserId == User.Identity.GetUserId() || ticket.OwnerUser.Email == User.Identity.Name || User.IsInRole("Admin") || projHelper.IsUserOnProject(User.Identity.GetUserId(),ticket.ProjectId))
            {
                return View(ticket);
            }

            return RedirectToAction("InvalidAttempt", "Home");
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create(int id)
        {
            if(projHelper.IsUserOnProject(User.Identity.GetUserId(), id))
            {
                ViewBag.ProjectId = new SelectList(db.Projects.Where(p => p.Id == id), "Id", "Name");
                ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
                ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
                ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
                return View();
            }
            return RedirectToAction("InvalidAttempt", "Home");
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

            if (ticket.AssignedToUserId == User.Identity.GetUserId() || ticket.OwnerUser.Email == User.Identity.Name || User.IsInRole("Admin") || projHelper.IsUserOnProject(User.Identity.GetUserId(),ticket.ProjectId))
            {
                ViewBag.AssignedToUserId = new SelectList(roleHelper.UsersInRole("Developer"), "Id", "FirstName", ticket.AssignedToUser);
                ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
                ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
                ViewBag.TicketStatusId = new SelectList(db.TicketStatuses.Where(s => s.Name != "New"), "Id", "Name", ticket.TicketStatusId);
                ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
                return View(ticket);
            }
            return RedirectToAction("InvalidAttempt","Home");
            
        }

        // GET: Tickets/Edit/5
        public ActionResult EditDash(int? id)
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

            if (ticket.AssignedToUserId == User.Identity.GetUserId() || ticket.OwnerUser.Email == User.Identity.Name || User.IsInRole("Admin") || projHelper.IsUserOnProject(User.Identity.GetUserId(), ticket.ProjectId))
            {
                ViewBag.AssignedToUserId = new SelectList(roleHelper.UsersInRole("Developer"), "Id", "FirstName", ticket.AssignedToUser);
                ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
                ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
                ViewBag.TicketStatusId = new SelectList(db.TicketStatuses.Where(s => s.Name != "New"), "Id", "Name", ticket.TicketStatusId);
                ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
                return View(ticket);
            }
            return RedirectToAction("InvalidAttempt", "Home");

        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Created,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var notificationHelper = new NotificationHelper();
                var historyHelper = new HistoryHelper();
                var currentStatus = ticket.TicketStatusId;

                if (currentStatus == 0)
                {
                    ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "In Progress").Id;
                }

                //reference old Ticket
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                if (User.Identity.GetUserId() == "db9a774b-807c-4b9b-9b22-34c191872996")
                {
                    if (ticket.AssignedToUserId == "5f84068f-4213-4d02-81a4-21936ae10cdc" || ticket.OwnerUserId == "60f316c5-536c-4f06-83d3-38a555febc29")
                    {
                        ticket.Updated = DateTime.Now;
                        db.Entry(ticket).State = EntityState.Modified;
                        db.SaveChanges();

                        //compare to the incoming Ticket (ticket)
                        notificationHelper.Notify2(oldTicket, ticket);
                        historyHelper.AddHistory(oldTicket, ticket);

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("InvalidAttempt", "Home");
                    }
                }

                ticket.Updated = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                //compare to the incoming Ticket (ticket)
                notificationHelper.Notify2(oldTicket,ticket);
                historyHelper.AddHistory(oldTicket, ticket);

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

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDash([Bind(Include = "Id,Title,Description,Created,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var notificationHelper = new NotificationHelper();
                var historyHelper = new HistoryHelper();
                var currentStatus = ticket.TicketStatusId;

                if (currentStatus == 0)
                {
                    ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "In Progress").Id;
                }

                //reference old Ticket
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                if (User.Identity.GetUserId() == "db9a774b-807c-4b9b-9b22-34c191872996")
                {
                    if (ticket.AssignedToUserId == "5f84068f-4213-4d02-81a4-21936ae10cdc" || ticket.OwnerUserId == "60f316c5-536c-4f06-83d3-38a555febc29")
                    {
                        ticket.Updated = DateTime.Now;
                        db.Entry(ticket).State = EntityState.Modified;
                        db.SaveChanges();

                        //compare to the incoming Ticket (ticket)
                        notificationHelper.Notify2(oldTicket, ticket);
                        historyHelper.AddHistory(oldTicket, ticket);

                        return RedirectToAction("TicketsDashboard");
                    }
                    else
                    {
                        return RedirectToAction("InvalidAttempt", "Home");
                    }
                }

                ticket.Updated = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                //compare to the incoming Ticket (ticket)
                notificationHelper.Notify2(oldTicket, ticket);
                historyHelper.AddHistory(oldTicket, ticket);

                return RedirectToAction("TicketsDashboard");
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
            if (User.Identity.GetUserId() == "db9a774b-807c-4b9b-9b22-34c191872996" || User.Identity.GetUserId() == "3eaa1491-7553-40fa-b7e1-b994e05d05e0" || User.Identity.GetUserId() == "5f84068f-4213-4d02-81a4-21936ae10cdc" || User.Identity.GetUserId() == "60f316c5-536c-4f06-83d3-38a555febc29")
            {
                return RedirectToAction("InvalidAttempt", "Home");
            }

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
