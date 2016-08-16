using ChieftensLMS.Classes;
using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using ChieftensLMS.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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

		public IList<string> GetRolesForUser(string id)
		{
			var _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			IList<string> currentUserRoles = _userManager.GetRoles(id);

			return currentUserRoles;
		}

		[Authorize]
		public ActionResult All()
		{
			string currentUserId = User.Identity.GetUserId();
			IList<string> currentUserRoles = GetRolesForUser(currentUserId);

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

		public ActionResult AllUsers(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid argument to api");

			var course = _courseService.GetById((int)id);

			if (course == null)
				return ApiResult.Fail("Course does not exist");

			return ApiResult.Success(_courseService.GetUsersForCourse(course));
		}

		public ActionResult Single(int? id)
		{

			if (id == null)
				return ApiResult.Fail("Invalid argument to api");

			var course = _courseService.GetById((int)id);

			if (course == null)
				return ApiResult.Fail("Course does not exist");

			string currentUserId = User.Identity.GetUserId();
			IList<string> currentUserRoles = GetRolesForUser(currentUserId);

			//kolla om studenten är i kursen
			if (currentUserRoles.Contains("Teacher") || _courseService.HasStudentWithId((int)id, currentUserId))
			{
				var returnData = new
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
			else
				return ApiResult.Fail("Current user not in Teacher/Student role");



		}

	}
}
