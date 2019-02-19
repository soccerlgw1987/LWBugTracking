using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LWBugTracking.Models
{
    public class ProjectTable
    {
        public ICollection<Project> Projects { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

        public ProjectTable()
        {
            this.Projects = new List<Project>();
            this.Tickets = new List<Ticket>();
        }
    }
}
