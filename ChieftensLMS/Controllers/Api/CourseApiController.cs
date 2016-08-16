using ChieftensLMS.Classes;
using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using ChieftensLMS.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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




		// All courses
		public ActionResult All()
		{
			var _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			string currentUserId = User.Identity.GetUserId();
			IList<string> currentUserRoles = _userManager.GetRoles(currentUserId);

			IEnumerable<Course> courseList = null;

			// Get list of courses a student is taking, or all for a teacher
			if (currentUserRoles.Contains("Teacher"))
				courseList = _courseService.GetAll();
			else if (currentUserRoles.Contains("Student"))
				courseList = _courseService.GetForUserId(currentUserId);
			else
				return ApiResult.Fail("Current user is not in Teacher/Student role");

			// Project list before returning
			var courses = courseList.Select(c => new
			{
				Id = c.Id,
				Name = c.Name,
				Description = c.Description
			});

			return ApiResult.Success(new { Courses = courses });
		}

		public ActionResult Single(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Hej");

			var course = _courseService.GetById((int)id);

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
