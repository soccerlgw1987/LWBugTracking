using LWBugTracking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LWBugTracking.Helper
{
    public class ProjectTableHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static ProjectTable GetProjectTableData()
        {
            return new ProjectTable
            {
                Projects = db.Projects.AsNoTracking().ToList(),
                Tickets = db.Tickets.AsNoTracking().ToList(),
                TicketComments = db.TicketComments.ToList(),
                TicketAttachments = db.TicketAttachments.ToList()
            };
        }

    }
}