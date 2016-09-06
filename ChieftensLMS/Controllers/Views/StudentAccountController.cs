using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChieftensLMS.Controllers.Views
{
	[Authorize(Roles = "Teacher")]
    public class StudentAccountController : Controller
    {
        // GET: StudentAccount
        public ActionResult Index()
        {

            return View();
        }
    }
}