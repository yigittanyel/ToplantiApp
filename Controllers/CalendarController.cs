using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Controls;
using DHTMLX.Scheduler.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ToplantiApp.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar
        public ActionResult Select()
        {
            // get available calendars for DropDownList
            var api = new OutlookCalendarApi(Request.Cookies["OutlookAccessToken"].Value);
            var calendars = api.GetCalendars().Select(c => new SelectListItem { Text = c.Name, Value = c.Id }).ToList();
            ViewBag.Calendars = calendars;

            return View();
        }
        public ActionResult Index()
        {
            var scheduler = ConfigureScheduler();
            return View(scheduler);
        }

        private DHXScheduler ConfigureScheduler()
        {
            var scheduler = new DHXScheduler();

            scheduler.LoadData = true;
            scheduler.EnableDataprocessor = true;

            string calendarId = Request["calendarId"];

            // load only this month events
            scheduler.EnableDynamicLoading(SchedulerDataLoader.DynamicalLoadingMode.Month);

            // compose data action for sending dateFrom, dateTo and accessToken
            scheduler.DataAction = String.Format(
                            "{0}?calendarId={1}",
                            Url.Action("Data", "DataAccess"),
                            Url.Encode(calendarId)
                            );


            scheduler.SaveAction = String.Format(
                "{0}?calendarid={1}",
                Url.Action("Save", "DataAccess"),
                Url.Encode(calendarId)
                );

            scheduler.Lightbox.Add(new LightboxText("text", "Name"));
            scheduler.Lightbox.Add(new LightboxText("description", "Details"));

            // TODO: implement!

            return scheduler;
        }
    }
}