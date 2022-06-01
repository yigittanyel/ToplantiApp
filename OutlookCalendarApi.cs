using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using ToplantiApp.Models;

namespace ToplantiApp
{
    public class OutlookCalendarApi
    {
        public string AccessToken { get; set; }
        
        private const string UrlBase = "https://apis.live.net/v5.0/";
        
        public OutlookCalendarApi()
        {
        }
        
        public OutlookCalendarApi(string accessToken)
        {
            AccessToken = accessToken;
        }

        private T GetResponse<T>(string uri, string method, Dictionary<string, object> data)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = method;
            request.Headers.Add("Authorization", string.Format("Bearer {0}", AccessToken));
            
             if (data.Count != 0)
            {
                string paramsStr = string.Join("&", data.Select(item => String.Format("{0}={1}", item.Key, item.Value)));
                request.ContentLength = paramsStr.Length;
                request.ContentType = "application/x-www-form-urlencoded";
                
                using (var sw = new StreamWriter(request.GetRequestStream()))
                {
                    sw.Write(paramsStr);
                    sw.Close();
                }
            }
            
                  var response = (HttpWebResponse)request.GetResponse();
            string responseText;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                responseText = reader.ReadToEnd();
            }
            
            return new JavaScriptSerializer().Deserialize<T>(responseText);
        }
        private T GetResponse<T>(string uri, string method)
        {
            return GetResponse<T>(uri, method, new Dictionary<string, object>());
        }
        public IEnumerable<OutlookCalendar> GetCalendars()
        {
            var data = GetResponse<ReceivedCalendarsData>(UrlBase + "me/calendars", "GET");
            return data.Data.Where(item => item.Permissions.Equals("owner"));
        }

        public string CreateEvent(OutlookEvent @event, string calendarId)
        {
            var sendData = EventAsSendData(@event);
            var receivedData = GetResponse<OutlookEvent>(UrlBase + calendarId + "/events", "POST", sendData);
            return receivedData.id;
        }
        
        public string UpdateEvent(OutlookEvent @event)
        {
            var sendData = EventAsSendData(@event);
            var receivedData = GetResponse<OutlookEvent>(UrlBase + @event.id, "PUT", sendData);
            return receivedData.id;
        }
        
        public IEnumerable<OutlookEvent> ReadEvents(string calendarId, DateTime from, DateTime to)
        {
            string requestStr = String.Format(
                "{0}{1}/events?start_time={2}Z&end_time={3}Z",
                UrlBase,
                calendarId,
                from.ToString("s"),
                to.ToString("s")
                );
            
                        var eventsData = GetResponse<ReceivedEventsData>(requestStr, "GET");
            return eventsData.Data;
        }
        
        public void DeleteEvent(string id)
        {
            GetResponse<object>(UrlBase + id, "DELETE");
        }

        private Dictionary<string, object> EventAsSendData(OutlookEvent @event)
        {
            return new Dictionary<string, object>
            {
                {"name", @event.name},
                {"description", @event.description ?? String.Empty},
                {"start_time", @event.start_time.ToUniversalTime().ToString("s") + "Z"},
                {"end_time", @event.end_time.ToUniversalTime().ToString("s") + "Z"}
            };
        }
    }
}