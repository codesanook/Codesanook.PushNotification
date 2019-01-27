using Orchard.UI.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeSanook.PushNotification.Controllers
{
    [Admin]//render in context of admin menu
    public class PushNotificationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}