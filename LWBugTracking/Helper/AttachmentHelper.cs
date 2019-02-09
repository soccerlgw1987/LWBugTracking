using LWBugTracking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LWBugTracking.Helper
{
    public class AttachmentHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static List<TicketAttachment> GetAttachments(int ticketId)
        {
            return db.TicketAttachments.AsNoTracking().Where(t => t.TicketId == ticketId).ToList();
        }
    }
}