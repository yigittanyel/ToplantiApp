using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToplantiApp.Models
{
    public class Toplanti
    {
        public int ToplantiId { get; set; }
        public DateTime BaslangicSaati { get; set; }
        public DateTime BitisSaati { get; set; }
        public  int ToplantiOdasiId{ get; set; }
        public  string EmailBody{ get; set; }
        public virtual ToplantiOdasi ToplantiOdasi { get; set; }
        public string Duzenleyen { get; set; }
        public string ToplantiKonusu { get; set; }

        public List<Toplanti> ReadMailItems()
        {
            Application outlookApplication = null;
            NameSpace outlookNamespace = null;
            MAPIFolder inboxFolder = null;
            Items mailItems = null;
            List<Toplanti> listEmailDetails = new List<Toplanti>();
            Toplanti emailDetails;
            try
            {
                outlookApplication = new Application();
                outlookNamespace = outlookApplication.GetNamespace("MAPI");
                inboxFolder = outlookNamespace.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
                mailItems = inboxFolder.Items;
                foreach(MailItem item in mailItems)
                {
                    emailDetails = new Toplanti();
                    emailDetails.Duzenleyen = item.SenderEmailAddress;
                    emailDetails.ToplantiKonusu = item.Subject;
                    emailDetails.EmailBody= item.Body;
                    listEmailDetails.Add(emailDetails);
                    ReleaseComObject(item);
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                ReleaseComObject(mailItems);
                ReleaseComObject(inboxFolder);
                ReleaseComObject(outlookNamespace);
                ReleaseComObject(outlookApplication);
            }
            return listEmailDetails;
        }
        private static void ReleaseComObject(object obj)
        {
            if(obj!= null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
        }
    }
}