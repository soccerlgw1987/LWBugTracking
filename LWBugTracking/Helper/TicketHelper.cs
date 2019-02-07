using LWBugTracking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace LWBugTracking.Helper
{
    public class TicketHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

        //public bool IsUserOnTicket(string userId, int ticketId)
        //{
        //    var ticket = db.Tickets.Find(ticketId);
        //    var flag = ticket.ApplicationUsers.Any(u => u.Id == userId);
        //    return (flag);
        //}

        //public ICollection<Ticket> ListUserTickets(string userId)
        //{
        //    ApplicationUser user = db.Users.Find(userId);

        //    var tickets = user.Tickets.ToList();

        //    return (tickets);
        //}

        //public void AddUserToProject(string userId, int projectId)
        //{
        //    if (!IsUserOnProject(userId, projectId))
        //    {
        //        Project proj = db.Projects.Find(projectId);
        //        var newUser = db.Users.Find(userId);

        //        proj.ApplicationUsers.Add(newUser);
        //        db.SaveChanges();
        //    }
        //}

        //public void RemoveUserFromProject(string userId, int projectId)
        //{
        //    if (IsUserOnProject(userId, projectId))
        //    {
        //        Project proj = db.Projects.Find(projectId);
        //        var delUser = db.Users.Find(userId);
        //        proj.ApplicationUsers.Remove(delUser);
        //        db.Entry(proj).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //}

        //public ICollection<ApplicationUser> UsersOnTicket(int projectId)
        //{
        //    return db.Tickets.Find(projectId).ApplicationUsers;
        //}

        //public ICollection<ApplicationUser> UsersNotOnProject(int projectId)
        //{
        //    return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        //}

        //public ICollection<string> GetProjectUserRoles(string roleName, int projectId)
        //{
        //    var users = UsersOnProject(projectId);
        //    var usersInRole = new List<string>();
        //    var roleHelper = new UserRolesHelper();

        //    foreach (var user in users)
        //    {
        //        if (roleHelper.IsUserInRole(user.Id, roleName))
        //        {
        //            usersInRole.Add(user.FirstName + " " + user.LastName);
        //        }
        //    }
        //    return usersInRole;
        //}
    }
}