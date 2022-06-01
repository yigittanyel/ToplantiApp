using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToplantiApp.Models
{
    public class Event
    {
        public string id { get; set; }
        public string text { get; set; }
        public string description { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
    }
}