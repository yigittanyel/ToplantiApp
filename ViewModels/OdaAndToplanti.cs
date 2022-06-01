using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToplantiApp.Models;

namespace ToplantiApp.ViewModels
{
    public class OdaAndToplanti
    {
        public IEnumerable<ToplantiOdasi> ToplantiOdasis { get; set; }
    }
}