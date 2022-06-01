using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ToplantiApp.Models
{
    public class Context:DbContext
    {
        public DbSet<Toplanti> Toplantis { get; set; } 
        public DbSet<ToplantiOdasi> ToplantiOdasis { get; set; } 
        public DbSet<POPEmail> POPEmails { get; set; } 
    }
}