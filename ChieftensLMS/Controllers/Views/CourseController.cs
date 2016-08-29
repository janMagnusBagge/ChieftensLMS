using ChieftensLMS.Classes;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace ChieftensLMS.Controllers
{
    public class CourseController : LMSController
    {
		public ActionResult Overview()
		{
			return View();
		}

		public ActionResult Users(int? id)
		{
			return View(id);
		}

		public ActionResult Details(int? id)
		{
			return View(id);
		}

	}
}
