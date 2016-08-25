using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChieftensLMS.Controllers.Views
{
    public class LecturesController : Controller
    {
        // GET: Lectures
        public ActionResult Index(int? id)
        {
            return View(id);
        }
    }
}