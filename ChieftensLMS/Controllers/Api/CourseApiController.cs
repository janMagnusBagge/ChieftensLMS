using ChieftensLMS.DAL;
using ChieftensLMS.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChieftensLMS.Controllers.Api
{
	public class CourseApiController : Controller
	{
		private LMSDbContext _context;
		private CourseService _courseService;

		public CourseApiController()
		{
			_context = new LMSDbContext();
			_courseService = new CourseService(_context);
		}

		// GET: Courses
		public ActionResult Index()
		{
			var courses = _courseService.GetCoursesForUserId(User.Identity.GetUserId())
				.Select(c => new
					{
						c.Id,
						c.Name,
						c.Description
					}
				);

			var returnObject = new
			{
				data = new { courses = courses },
				success = true
			};

			return Json(returnObject, JsonRequestBehavior.AllowGet);
		}

		//// GET: Courses/Details/5
		//public ActionResult Details(int? id)
		//{
		//	if (id == null)
		//	{
		//		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//	}

		//	var course = _courseService.GetCourseById((int)id);

		//	if (course == null)
		//	{
		//		return HttpNotFound();
		//	}
		//	return View(course);
		//}

	}
}
