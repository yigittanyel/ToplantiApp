using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToplantiApp.Models;
using ToplantiApp.ViewModels;
using System.Threading.Tasks;
using DHTMLX.Scheduler.Data;
using DHTMLX.Common;
using OpenPop.Mime;
using OpenPop.Pop3;
using System.Configuration;

namespace ToplantiApp.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        Context c = new Context();
        public ActionResult Index()
        {
            OdaAndToplanti oat = new OdaAndToplanti();
            oat.ToplantiOdasis = c.ToplantiOdasis.ToList();
            var oda = c.ToplantiOdasis.FirstOrDefault(q => q.ToplantiOdasiId == 1);
            ViewBag.odaAdi = oda.OdaAdi;
            return View(oat);
        }
        public ActionResult Kozahan()
        {
            var deger2 = c.Toplantis.Where(a => a.ToplantiOdasiId == 1).FirstOrDefault();
            var oda = c.ToplantiOdasis.FirstOrDefault(q => q.ToplantiOdasiId == 1);
            ViewBag.odaAdi = oda.OdaAdi;
            return View("Kozahan", deger2);
        }
        public ActionResult Topkapı()
        {
            var deger2 = c.Toplantis.Where(a => a.ToplantiOdasiId == 3).FirstOrDefault();
            var oda = c.ToplantiOdasis.FirstOrDefault(q => q.ToplantiOdasiId == 3);
            ViewBag.odaAdi = oda.OdaAdi;
            return View("Topkapı", deger2);
        }
        public ActionResult Fidanhan()
        {
            var deger2 = c.Toplantis.Where(a => a.ToplantiOdasiId == 2).FirstOrDefault();
            var oda = c.ToplantiOdasis.FirstOrDefault(q => q.ToplantiOdasiId == 2);
            ViewBag.odaAdi = oda.OdaAdi;
            return View("Fidanhan", deger2);
        }

        public ActionResult Popmail()
        {
            var deger1 = c.Toplantis.Where(a => a.ToplantiOdasiId == 1).FirstOrDefault();
            var deger2 = c.POPEmails.Where(a => a.ToplantiOdasiId == 1).FirstOrDefault();

            Pop3Client pop3client = new Pop3Client();

            //var username = ConfigurationManager.AppSettings["kamuran.canakli@ibras.com.tr"];
            //var password = ConfigurationManager.AppSettings["Qkf87h.773!"];

            //var username = ConfigurationManager.AppSettings["username"];
            //var password = ConfigurationManager.AppSettings["password"];

            pop3client.Connect("outlook.office365.com", 995, true);
            pop3client.Authenticate("beylerbeyi@ibras.com.tr", "bylr.2022@", AuthenticationMethod.UsernameAndPassword);

            int count = pop3client.GetMessageCount(); //total count of emaik in messageBox in our inbox
            var Emails = new List<POPEmail>();

            int counter = 0;
            for (int i = count; i >= 1; i--)
            {
                Message message = pop3client.GetMessage(i);
                POPEmail email = new POPEmail()
                {
                    MessageNumber = i,
                    Subject = message.Headers.Subject,
                    DataSent = message.Headers.DateSent,
                    From = string.Format("<a href = 'mailto:{1}'>{0}</a>", message.Headers.From.DisplayName, message.Headers.From.Address),
                };
                MessagePart body = message.FindFirstHtmlVersion();
                if (body != null)
                {
                    email.Body = body.GetBodyAsText();
                }
                else
                {
                    body = message.FindFirstPlainTextVersion();
                    if (body != null)
                    {
                        email.Body = body.GetBodyAsText();
                    }
                }
                List<MessagePart> attachments = message.FindAllAttachments();

                foreach (MessagePart attachment in attachments)
                {
                    email.Attachments.Add(new Attachment
                    {
                        FileName = attachment.FileName,
                        ContentType = attachment.ContentType.MediaType,
                        Content = attachment.Body
                    });
                }
                Emails.Add(email);
                counter++;
                if (counter > 2)
                {
                    break;
                }
            }
            var emails = Emails;
            return View(emails);

        }


            public ActionResult Data()
        {
            var api = new OutlookCalendarApi
            {
                AccessToken = Request.Cookies["OutlookAccessToken"].Value
            };

            var events = api.ReadEvents(
                Request["calendarId"],
                DateTime.Parse(Request.QueryString["from"]),
                DateTime.Parse(Request.QueryString["to"])
                );

            return new SchedulerAjaxData(events.Select(item => new Event
            {
                id = item.id,
                text = item.name,
                description = item.description,
                start_date = item.start_time,
                end_date = item.end_time
            }));
        }

        public ActionResult Save(string id, FormCollection actionValues)
        {
            var action = new DataAction<string>(actionValues);
            var @event = DHXEventsHelper.Bind<Event>(actionValues);

            try
            {
                var api = new OutlookCalendarApi
                {
                    AccessToken = Request.Cookies["OutlookAccessToken"].Value
                };

                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        action.TargetId = api.CreateEvent(EventToOutlookEvent(@event), Request["calendarId"]);
                        break;
                    case DataActionTypes.Update:
                        api.UpdateEvent(EventToOutlookEvent(@event));
                        break;
                    case DataActionTypes.Delete:
                        api.DeleteEvent(id);
                        break;
                }
            }
            catch
            {
                action.Type = DataActionTypes.Error;
            }

            return new AjaxSaveResponse(action);
        }

        private OutlookEvent EventToOutlookEvent(Event @event)
        {
            return new OutlookEvent
            {
                description = @event.description,
                name = @event.text,
                start_time = @event.start_date,
                end_time = @event.end_date,
                id = @event.id
            };
        }

    }
}