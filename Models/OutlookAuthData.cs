using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToplantiApp.Models
{
    public class OutlookAuthData
    {
        public Uri RedirectUri { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public IEnumerable<string> Scopes { get; set; }
    }
}