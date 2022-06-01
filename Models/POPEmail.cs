using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ToplantiApp.Models
{
    [Serializable]
    public class POPEmail
    {
        public POPEmail()
        {
            this.Attachments = new List<Attachment>();
        }
        [Key]
        public int Id { get; set; }
        public int MessageNumber { get; set; }
        [AllowHtml]
        public string From { get; set; }
        [AllowHtml]
        public string Subject { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public DateTime DataSent { get; set; }
        public int? ToplantiOdasiId { get; set; }
        public virtual ToplantiOdasi ToplantiOdasi { get; set; }
        [AllowHtml]
        public List<Attachment> Attachments { get; set; }
    }
    [Serializable]
    public class Attachment
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}