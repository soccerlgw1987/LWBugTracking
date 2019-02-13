using Microsoft.AspNet.Identity;
using LWBugTracking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;

namespace LWBugTracking.Helper
{
    public class HistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void AddHistory(Ticket oldTicket, Ticket newTicket)
        {

            foreach (var propObj1 in newTicket.GetType().GetProperties())
            {
                //If the current property is NOT one of the properties I am interested in then move on...
                var trackedProperties = WebConfigurationManager.AppSettings["propertyHistory"].Split(',').ToList();
                if (!trackedProperties.Contains(propObj1.Name))
                    continue;

                //Else generate a TicketHistory record
                var oldTicketProp = oldTicket.GetType().GetProperty(propObj1.Name);
                var newTicketProp = newTicket.GetType().GetProperty(propObj1.Name);

                var oldPropValue = propObj1.GetValue(oldTicket, null);
                var newPropValue = propObj1.GetValue(newTicket, null);

                if (oldPropValue != newPropValue)
                {
                    var historyRecord = new TicketHistory
                    {
                        PropertyName = propObj1.Name,
                        OldValue = oldPropValue.ToString(),
                        NewValue = newPropValue.ToString(),
                        Changed = DateTime.Now,
                        TicketId = newTicket.Id,
                        UserId = HttpContext.Current.User.Identity.GetUserId()
                    };

                    db.TicketHistories.Add(historyRecord);
                    db.SaveChanges();
                }
            }
        }
    }
}