using ChieftensLMS.Classes;
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

			var returnData = new { courses = courses };

			return ApiResult.Success(returnData);
		}

		public ActionResult Details(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Hej");

			var course = _courseService.GetCourseById((int)id);

			if (course == null)
				return ApiResult.Fail("Hej");

			var
				returnData = new
				{
					course = new
					{
						Id = course.Id,
						Name = course.Name,
						Description = course.Description
					}
			};

			return ApiResult.Success(returnData);
		}

	}
}
