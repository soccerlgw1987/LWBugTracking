using LWBugTracking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LWBugTracking.Helper
{
    public class LookupData
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static string GetNameFromId(string propertyName, string id)
        {
            var name = "";

            switch(propertyName)
            {
                case "TicketPriorityId":
                    name = db.TicketPriorities.Find(Convert.ToInt32(id)).Name;
                    break;
                case "TicketStatusId":
                    name = db.TicketStatuses.Find(Convert.ToInt32(id)).Name;
                    break;
                case "TicketTypeId":
                    name = db.TicketTypes.Find(Convert.ToInt32(id)).Name;
                    break;
                case "AssignedToUserId":
                    name = db.Users.Find(id).FirstName + " " + db.Users.Find(id).LastName;
                    break;
                //case "Title":
                //    name = db.Tickets.Find(id).Title;
                //    break;
                //case "Description":
                //    name = db.Tickets.Find(id).Description;
                //    break;
            }

            return name;
        }
    }
}