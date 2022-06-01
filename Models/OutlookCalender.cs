using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToplantiApp.Models
{
    public class OutlookCalendar
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Permissions { get; set; }
    }
    public class OutlookEvent
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
    }

    internal class ReceivedCalendarsData
    {
        public List<OutlookCalendar> Data { get; set; }
    }

    internal class ReceivedEventsData
    {
        public List<OutlookEvent> Data { get; set; }
    }
}