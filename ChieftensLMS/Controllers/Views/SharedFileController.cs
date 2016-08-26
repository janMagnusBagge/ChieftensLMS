using ChieftensLMS.Classes;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Web.Mvc;

namespace ChieftensLMS.Controllers
{
    public class SharedFileController : LMSController
    {
		public ActionResult ForCourse(int? id)
        {
			if (_userManager.IsInRole(_currentUserId, "Teacher"))
				return View("ForCourse_Teacher", id);
			else
				return View(id);
		}

		public ActionResult Mine()
		{
			return View();
		}

		public ActionResult Upload(int? id)
		{
			return View(id);
		}

		public ActionResult CourseOverview(int? id)
		{
			return View(id);
		}
	}
}
