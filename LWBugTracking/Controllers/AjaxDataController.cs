using LWBugTracking.Models;
using LWBugTracking.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;

namespace LWBugTracking.Controllers
{
    public class AjaxDataController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: AjaxDonutChartData
        public JsonResult GetDonutChartData()
        {
            var projectStat = new ProjectTable();
            var dataChart = new List<MorrisChartData>();

            foreach(var item in db.Projects.GroupBy(p => p.ProjectStatusId).Select(data => new { Id = data.Key, RecordCount = data.Count().ToString()}).Where(s => s.Id != 4).ToList())
            {
                dataChart.Add(new MorrisChartData
                {
                    label = db.ProjectStatuses.Find(item.Id).Status,
                    value = item.RecordCount
                });
            }

            return Json(dataChart);
        }

        public JsonResult GetBarChartData()
        {
            var dataBar = new List<Object[]>();
            var projectStat = new ProjectTable();

            foreach (var item in db.Tickets.Where(s => s.TicketStatusId != 5).GroupBy(p => p.TicketPriorityId).Select(data => new { Id = data.Key, RecordCount = data.Count().ToString() }).ToList())
            {
                dataBar.Add(new Object[]
                {
                    db.TicketPriorities.Find(item.Id).Name,
                    item.RecordCount
                });
            }

            return Json(dataBar);
        }

        public JsonResult GetSparkBarChartData()
        {
            var dataBar = new List<Object[]>();
            var projectStat = new ProjectTable();

            foreach (var item in db.Tickets.Where(s => s.TicketStatusId != 5).ToList())
            {
                dataBar.Add(new Object[]
                {
                    item.TicketComments.Count()
                });
            }

            return Json(dataBar);
        }

        public JsonResult GetSparkLineChartData()
        {
            var dataBar = new List<Object[]>();
            var projectStat = new ProjectTable();

            foreach (var item in db.Tickets.Where(s => s.TicketStatusId != 5).ToList())
            {
                dataBar.Add(new Object[]
                {
                    item.TicketAttachments.Count()
                });
            }

            return Json(dataBar);
        }

        public JsonResult GetDoubleLineChartData()
        {
            var dataChart = new List<DoubleChart>();
            var roleHelper = new UserRolesHelper();

            foreach (var item in roleHelper.UsersInRole("Developer"))
            {
                dataChart.Add(new DoubleChart
                {
                    y = item.FirstName,
                    a = item.Projects.Where(s => s.ProjectStatusId != 4).Count(),
                    b = db.Tickets.Where(s => s.TicketStatusId != 5).Count()
                });
            }

            return Json(dataChart);
        }

        public JsonResult GetLineChartData()
        {
            var dataBar = new List<Object[]>();
            var roleHelper = new UserRolesHelper();
            var projHelper = new Project();

            foreach (var item in roleHelper.UsersInRole("Submitter"))
            {
                dataBar.Add(new Object[]
                {
                    item.FirstName,
                    item.Projects.Where(s => s.ProjectStatusId != 4).Count()
                });
            }

            return Json(dataBar);
        }
    }
}