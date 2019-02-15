using LWBugTracking.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace LWBugTracking.Helper
{
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //public async Task Notify(Ticket oldTicket, Ticket newTicket)
        //{
        //    var notificationType = DetermineNotificationType(oldTicket.AssignedToUserId, newTicket.AssignedToUserId);

        //    if (notificationType != NotificationType.None)
        //    {
        //        await SendEmailNotification(newTicket.Id, oldTicket.AssignedToUserId, newTicket.AssignedToUserId, notificationType);
        //        AddNotification(newTicket.Id, oldTicket.AssignedToUserId, newTicket.AssignedToUserId, notificationType);
        //    }
        //}

        //public NotificationType DetermineNotificationType(string oldDevId, string newDevId)
        //{
        //    var type = NotificationType.None;

        //    if (oldDevId == null && newDevId != null)
        //        type = NotificationType.Assigned;

        //    if (oldDevId != null && newDevId == null)
        //        type = NotificationType.UnAssigned;

        //    if ((oldDevId != null && newDevId != null) && (oldDevId != newDevId))
        //        type = NotificationType.ReAssigned;

        //    return type;
        //}

        //public async Task SendEmailNotification(int ticketId, string oldUserId, string newUserId, NotificationType type)
        //{
        //    string from = $"LWBugTracking <LWBugTracking@mailinator.com>";
        //    string subject = "", body = "", emailTo = "", nameTo = "";
        //    var ticketTitle = db.Tickets.Find(ticketId);

        //    switch (type)
        //    {
        //        case NotificationType.Assigned:
        //            subject = "Ticket Assignment";
        //            body = $"You have been Assigned to the following Ticket: {ticketTitle.Title}";

        //            var newUser = db.Users.Find(newUserId);
        //            emailTo = newUser.Email;
        //            nameTo = newUser.FirstName;

        //            var assignedMailModel = new EmailModel
        //            {
        //                FromEmail = from,
        //                ToEmail = emailTo,
        //                ToName = nameTo,
        //                Body = body,
        //                Subject = subject
        //            };

        //            await SendEmail(assignedMailModel);

        //            break;
        //        case NotificationType.UnAssigned:
        //            subject = "Ticket UnAssignment";
        //            body = $"You have been Unassigned from the following Ticket: {ticketTitle.Title}";
        //            emailTo = db.Users.Find(oldUserId).Email;

        //            var oldUser = db.Users.Find(newUserId);
        //            emailTo = oldUser.Email;
        //            nameTo = oldUser.FirstName;

        //            var unAssignedMailModel = new EmailModel
        //            {
        //                FromEmail = from,
        //                ToEmail = emailTo,
        //                ToName = nameTo,
        //                Body = body,
        //                Subject = subject
        //            };

        //            await SendEmail(unAssignedMailModel);

        //            break;
        //        case NotificationType.ReAssigned:
        //            subject = "Ticket Reassignment";

        //            body = $"You have been Assigned to the following Ticket: {ticketTitle.Title}";
        //            emailTo = db.Users.Find(newUserId).Email;

        //            var reAssignedNewUser = db.Users.Find(newUserId);
        //            emailTo = reAssignedNewUser.Email;
        //            nameTo = reAssignedNewUser.FirstName;

        //            var reAssignedNewUserMailModel = new EmailModel
        //            {
        //                FromEmail = from,
        //                ToEmail = emailTo,
        //                ToName = nameTo,
        //                Body = body,
        //                Subject = subject
        //            };

        //            await SendEmail(reAssignedNewUserMailModel);

        //            body = $"You have been Unassigned from the following Ticket: {ticketTitle.Title}";
        //            emailTo = db.Users.Find(oldUserId).Email;

        //            var reAssignedOldUser = db.Users.Find(oldUserId);
        //            emailTo = reAssignedOldUser.Email;
        //            nameTo = reAssignedOldUser.FirstName;

        //            var reAssignedoldUserMailModel = new EmailModel
        //            {
        //                FromEmail = from,
        //                ToEmail = emailTo,
        //                ToName = nameTo,
        //                Body = body,
        //                Subject = subject
        //            };

        //            await SendEmail(reAssignedoldUserMailModel);

        //            break;
        //    }
        //}

        //public async Task SendEmail(EmailModel emailData)
        //{
        //    var email = new MailMessage(emailData.FromEmail, emailData.ToEmail)
        //    {
        //        Subject = emailData.Subject,
        //        Body = emailData.Body,
        //        IsBodyHtml = true
        //    };

        //    var svc = new PersonalEmail();
        //    await svc.SendAsync(email);
        //}

        //public void AddNotification(int ticketId, string oldUserId, string newUserId, NotificationType type)
        //{

        //}

        //public enum NotificationType
        //{
        //    Assigned,
        //    UnAssigned,
        //    ReAssigned,
        //    None
        //}




        //added to make things easier to understand
        public void Notify2(Ticket oldTicket, Ticket newTicket)
        {
            //the purpose of this method is to determine whether or not the system needs to generate TicketNotification records
            var oldUserId = oldTicket.AssignedToUserId;
            var newUserId = newTicket.AssignedToUserId;

            //if the old and new are same, there is nothing to do
            if (oldUserId == newUserId)
                return;

            //if here, i am making a notification
            var newNotification = new TicketNotification
            {
                TicketId = newTicket.Id,
                Created = DateTime.Now
            };

            //Condition 1: the ticket is newly assigned. This means that the oldTicket has an AssignedToUserId = null and the newTicket has someone assigned
            if (oldUserId == null && newUserId != null)
            {
                //trigger an assignment
                newNotification.RecipientId = newUserId;
                newNotification.NotificationBody = $"You have been assigned to Ticket {newTicket.Id}";
                db.TicketNotifications.Add(newNotification);

                db.SaveChanges();
            }

            //Condition 2: The ticket has been newly unassigned. This means that the oldTicket had a AssignedToUserId and the newTicket does not
            else if (oldUserId != null && newUserId == null)
            {
                //trigger an unassignment
                newNotification.RecipientId = oldUserId;
                newNotification.NotificationBody = $"You have been unassigned from Ticket {newTicket.Id}";
                db.TicketNotifications.Add(newNotification);

                db.SaveChanges();
            }

            //Condition 3: Neither the oldTicket nor the newTicket AssignedToUserId's are null and they are different. This means the ticket was reassigned
            else
            {
                //trigger both assign and unassign
                newNotification.RecipientId = newUserId;
                newNotification.NotificationBody = $"You have been assigned to Ticket {newTicket.Id}";
                db.TicketNotifications.Add(newNotification);
                var secondNotification = new TicketNotification
                {
                    Created = DateTime.Now,
                    TicketId = newTicket.Id,
                    RecipientId = oldUserId,
                    NotificationBody = $"You have been unassigned from Ticket {newTicket.Id}"

                };

                db.TicketNotifications.Add(secondNotification);
                db.SaveChanges();
            }
        }

        public int GetNotificationCount()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            return db.TicketNotifications.Where(n => n.RecipientId == userId).Count();
        }

        public int GetUnreadNotificationCount()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            return db.TicketNotifications.Where(n => n.RecipientId == userId && n.Read == false).Count();
        }

        public List<TicketNotification> GetNewNotifications()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            return db.TicketNotifications.Where(n => n.RecipientId == userId && n.Read == false).OrderByDescending(n => n.Created).ToList();
        }

        public void GetAttachmentNotification(int ticketId)
        {
            var newNotification = new TicketNotification
            {
                TicketId = ticketId,
                Created = DateTime.Now
            };
            var assignedDev = db.Tickets.Find(ticketId).AssignedToUserId;

            newNotification.RecipientId = assignedDev;
            newNotification.NotificationBody = $"An attachment has been uploaded to Ticket {ticketId}";
            db.TicketNotifications.Add(newNotification);

            db.SaveChanges();
        }

        public void GetCommentNotification(int ticketId)
        {
            var newNotification = new TicketNotification
            {
                TicketId = ticketId,
                Created = DateTime.Now
            };
            var assignedDev = db.Tickets.Find(ticketId).AssignedToUserId;

            newNotification.RecipientId = assignedDev;
            newNotification.NotificationBody = $"A comment has been uploaded to Ticket {ticketId}";
            db.TicketNotifications.Add(newNotification);

            db.SaveChanges();
        }

    }
}