using LWBugTracking.Models;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LWBugTracking.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //public ActionResult Search()
        //{
        //    return View();
        //}


        

        public ActionResult Search(int? page, string searchStr)
        {
            ViewBag.search = searchStr;
            var bugList = FullSearch(searchStr);
            
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            
            return View(bugList.ToPagedList(pageNumber, pageSize));
        }


        public IQueryable<Project> FullSearch(string searchStr)
        {
            IQueryable<Project> result = null;
            if (searchStr != null)
            {
                result = db.Projects.AsQueryable();
                result = result.Where(p => p.Description.Contains(searchStr) ||
                    p.Name.Contains(searchStr) ||
                    p.Tickets.Any(c => c.Title.Contains(searchStr) ||
                    c.Description.Contains(searchStr)) ||
                    p.ApplicationUsers.Any(u => u.DisplayName.Contains(searchStr)));
            }
            else
            {
                result = db.Projects.AsQueryable();
            }
            return result;
        }
    }
}