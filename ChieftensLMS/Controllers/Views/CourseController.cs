using ChieftensLMS.Classes;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace ChieftensLMS.Controllers
{
    public class CourseController : LMSController
    {

        public ActionResult Mine()
		{ 
			return View();
        }

		public ActionResult All()
		{
			return View();
		}

		[Authorize]
		public ActionResult AllUsers(int? id)
		{
			// Teachers have a special view with more options
			if (_userManager.IsInRole(_currentUserId, "Teacher"))
				return View("AllUsers_Teacher", id);
			else
				return View(id);
		}


		public ActionResult Single(int? id)
		{
			return View(id);
		}

		[Authorize(Roles = "Teacher")]
		public ActionResult AddUser(int? id)
		{
			return View("AddUser_Teacher", id);
		}

		[Authorize(Roles = "Teacher")]
		public ActionResult AddCourse()
		{
			return View();
		}

	}
}
