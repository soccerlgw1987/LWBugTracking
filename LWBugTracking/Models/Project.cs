﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LWBugTracking.Models
{
    public class Project
    {
        public int Id { get; set; }
        public int ProjectStatusId { get; set; }
        public string Name { get; set; }
        //[AllowHtml]
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime CompletionDate { get; set; }
        
        public virtual ProjectStatus ProjectStatus { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public Project()
        {
            this.Tickets = new HashSet<Ticket>();
            this.ApplicationUsers = new HashSet<ApplicationUser>();
        }

    }
}