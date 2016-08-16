using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChieftensLMS.Controllers.Views
{
    public class DebugController : Controller
    {
        // GET: Debug
        public ActionResult AllUsers()
        {
            return View();
        }
    }
}