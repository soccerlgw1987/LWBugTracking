using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LWBugTracking.Models;

namespace LWBugTracking.Helper
{
    public class ProjectHelper
    {

        ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var flag = project.ApplicationUsers.Any(u => u.Id == userId);
            return (flag);
        }

        public ICollection<Project> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);

            var projects = user.Projects.ToList();

            return (projects);
        }

        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var newUser = db.Users.Find(userId);

                proj.ApplicationUsers.Add(newUser);
                db.SaveChanges();
            }
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var delUser = db.Users.Find(userId);
                proj.ApplicationUsers.Remove(delUser);
                db.Entry(proj).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ICollection<ApplicationUser> UsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).ApplicationUsers;
        }

        public ICollection<ApplicationUser> UsersNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }

        public ICollection<string> GetProjectUserRoles(string roleName, int projectId)
        {
            var users = UsersOnProject(projectId);
            var usersInRole = new List<string>();
            var roleHelper = new UserRolesHelper();

            foreach(var user in users)
            {
                if(roleHelper.IsUserInRole(user.Id, roleName))
                {
                    usersInRole.Add(user.FirstName + " " + user.LastName);
                }
            }
            return usersInRole;
        }

    }
}
