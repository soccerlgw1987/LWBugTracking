namespace LWBugTracking.Migrations
{
    using LWBugTracking.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LWBugTracking.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LWBugTracking.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.TicketPriorities.AddOrUpdate(p => p.Name,
                new TicketPriority() { Name = "None" },
                new TicketPriority() { Name = "Low" },
                new TicketPriority() { Name = "Medium" },
                new TicketPriority() { Name = "High" },
                new TicketPriority() { Name = "Urgent" });

            context.TicketStatuses.AddOrUpdate(p => p.Name,
                new TicketStatus() { Name = "New" },
                new TicketStatus() { Name = "In Progress" },
                new TicketStatus() { Name = "On Hold" },
                new TicketStatus() { Name = "Awaiting User" },
                new TicketStatus() { Name = "Complete" });

            context.TicketTypes.AddOrUpdate(p => p.Name,
                new TicketType() { Name = "Bug" },
                new TicketType() { Name = "Error" },
                new TicketType() { Name = "Critical" },
                new TicketType() { Name = "Other" });

            context.ProjectStatuses.AddOrUpdate(p => p.Status,
                new ProjectStatus() { Status = "New" },
                new ProjectStatus() { Status = "In Progress" },
                new ProjectStatus() { Status = "Past Due!" },
                new ProjectStatus() { Status = "Completed" });


            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //Add the admin user
            if (!context.Users.Any(u => u.Email == "soccerlgw1987@yahoo.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "soccerlgw1987@yahoo.com",
                    Email = "soccerlgw1987@yahoo.com",
                    FirstName = "Landon",
                    LastName = "Wyant",
                    DisplayName = "Landon"
                },
                "test87");
            }

            //Associate a user with a role
            var userId = userManager.FindByEmail("soccerlgw1987@yahoo.com").Id;
            userManager.AddToRole(userId, "Admin");

            if (!context.Users.Any(u => u.Email == "yijing@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "yijing@mailinator.com",
                    Email = "yijing@mailinator.com",
                    FirstName = "Yijing",
                    LastName = "Wyant",
                    DisplayName = "Teng"
                },
                "test87");
            }

            //Associate a user with a role
            userId = userManager.FindByEmail("yijing@mailinator.com").Id;
            userManager.AddToRole(userId, "Project Manager");

            if (!context.Users.Any(u => u.Email == "ella@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ella@mailinator.com",
                    Email = "ella@mailinator.com",
                    FirstName = "Ella",
                    LastName = "Wyant",
                    DisplayName = "Wife"
                },
                "test87");
            }

            //Associate a user with a role
            userId = userManager.FindByEmail("ella@mailinator.com").Id;
            userManager.AddToRole(userId, "Project Manager");

            if (!context.Users.Any(u => u.Email == "emma@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "emma@mailinator.com",
                    Email = "emma@mailinator.com",
                    FirstName = "Emma",
                    LastName = "Wyant",
                    DisplayName = "Child"
                },
                "test87");
            }

            //Associate a user with a role
            userId = userManager.FindByEmail("emma@mailinator.com").Id;
            userManager.AddToRole(userId, "Submitter");

            if (!context.Users.Any(u => u.Email == "tara@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "tara@mailinator.com",
                    Email = "tara@mailinator.com",
                    FirstName = "Tara",
                    LastName = "Wyant",
                    DisplayName = "The_Cat"
                },
                "test87");
            }

            //Associate a user with a role
            userId = userManager.FindByEmail("tara@mailinator.com").Id;
            userManager.AddToRole(userId, "Submitter");

            if (!context.Users.Any(u => u.Email == "wayne@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "wayne@mailinator.com",
                    Email = "wayne@mailinator.com",
                    FirstName = "Wayne",
                    LastName = "Turner",
                    DisplayName = "Bloodhog"
                },
                "test87");
            }

            //Associate a user with a role
            userId = userManager.FindByEmail("wayne@mailinator.com").Id;
            userManager.AddToRole(userId, "Developer");

            if (!context.Users.Any(u => u.Email == "casey@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "casey@mailinator.com",
                    Email = "casey@mailinator.com",
                    FirstName = "Casey",
                    LastName = "Anderson",
                    DisplayName = "Friend"
                },
                "test87");
            }

            //Associate a user with a role
            userId = userManager.FindByEmail("casey@mailinator.com").Id;
            userManager.AddToRole(userId, "Developer");

            if (!context.Users.Any(u => u.Email == "sandy@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sandy@mailinator.com",
                    Email = "sandy@mailinator.com",
                    FirstName = "Sandy",
                    LastName = "Turner",
                    DisplayName = "Cousin"
                },
                "test87");
            }

            //Associate a user with a role
            userId = userManager.FindByEmail("sandy@mailinator.com").Id;
            userManager.AddToRole(userId, "Developer");

        }
    }
}
