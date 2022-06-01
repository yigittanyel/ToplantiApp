using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToplantiApp.Models
{
    public class ToplantiOdasi
    {
        public int ToplantiOdasiId { get; set; }
        public string OdaAdi { get; set; }
        public ICollection<Toplanti> Toplantis { get; set; }
        public ICollection<POPEmail> POPEmails { get; set; }
    }
}