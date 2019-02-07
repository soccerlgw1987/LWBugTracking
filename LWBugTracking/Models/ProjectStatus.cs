using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LWBugTracking.Models
{
    public class ProjectStatus
    {


        public int Id { get; set; }

        public string Status { get; set; }

        public virtual ICollection<Project> Projects { get; set;}

        public ProjectStatus()
        {
            this.Projects = new HashSet<Project>();
        }
    }
}