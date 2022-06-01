//using System;
//using System.Collections.Generic;

//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using ToplantiApp.Models;

//namespace ToplantiApp.Controllers
//{
//    public class AuthController : Controller
//    {
//        // GET: Auth
//        public ActionResult Index()
//        {
//            var authHelper = new MvcAuthHelper(HttpContext, GetAuthData());
            
//            // authorize if not authorized
//            if (!authHelper.IsAuthorized())
//                return (ActionResult)authHelper.Authorize();
//                // sets cookie with token
//            var tokenCookie = new HttpCookie("OutlookAccessToken", authHelper.GetAccessToken());
//            tokenCookie.Expires = DateTime.Now.AddHours(1);
//            HttpContext.Response.SetCookie(tokenCookie);
            
//            return RedirectToAction("Select", "Calendar");
//        }

//        private OutlookAuthData GetAuthData()
//        {
//            return new OutlookAuthData
//            {
//                ClientId = System.Configuration.ConfigurationManager.AppSettings["OutlookApi_ClientId"],
//                ClientSecret = System.Configuration.ConfigurationManager.AppSettings["OutlookApi_ClientSecret"],
//                RedirectUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OutlookApi_RedirectUri"]),
//                Scopes = new[] { "wl.calendars_update", "wl.signin", "wl.events_create" }
//            };
//        }
//    }
//}