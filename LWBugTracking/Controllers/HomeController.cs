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

        //Admin
        public ActionResult Index()
        {
            return View();
        }

        //ProjectManager
        public ActionResult IndexPM()
        {
            return View();
        }

        //Developer
        public ActionResult IndexDev()
        {
            return View();
        }

        //Submitter
        public ActionResult IndexSub()
        {
            return View();
        }

        public ActionResult InvalidAttempt()
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
            EmailModel model = new EmailModel();

            return View(model);
        }

        //public ActionResult Search()
        //{
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email from: <bold>{0}</bold>({1})</p><p>Message:</p><p>{2}</p>";
                    var emailTo = ConfigurationManager.AppSettings["emailTo"];
                    var from = string.Format("LWBugTracking<{0}>", emailTo);
                    model.Body = model.Body;
                    var htmlBody = new MvcHtmlString(model.Body);
                    var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = model.Subject,
                        Body = string.Format(body, model.FromEmail, model.FromEmail, htmlBody),
                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);

                    return View(new EmailModel());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }



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