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
                Projects = db.Projects.ToList(),
                Tickets = db.Tickets.ToList()
            };
        }

    }
}