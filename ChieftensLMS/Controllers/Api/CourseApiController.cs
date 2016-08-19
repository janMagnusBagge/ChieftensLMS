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
		private ApplicationDbContext _context;
		private CourseService _courseService;

		public CourseApiController()
		{
			_context = new ApplicationDbContext();
			_courseService = new CourseService(_context);
		}

		public IList<string> GetRolesForUser(string id)
		{
			var _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			IList<string> currentUserRoles = _userManager.GetRoles(id);

			return currentUserRoles;
		}

		[Authorize(Roles = "Teacher")]
		public ActionResult All()
		{
			var courses = _courseService.GetAll().Select(c => new
			{
				c.Id,
				c.Name,
				c.Description
			});

			return ApiResult.Success(new { Courses = courses });
		}

		[Authorize]
		public ActionResult Mine()
		{
			string currentUserId = User.Identity.GetUserId();
			
			// Project list before returning
			var courses = _courseService.GetForUserId(currentUserId).Select(c => new
			{
				c.Id,
				c.Name,
				c.Description
			});

			return ApiResult.Success(new { Courses = courses });
		}

		[Authorize]
		public ActionResult AllUsers(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid argument to api");

			Course course = _courseService.GetById((int)id);

			if (course == null)
				return ApiResult.Fail("Course does not exist");
		
			var returnData = _courseService.GetUsersForCourse(course).Select(c => new
			{
				c.Name,
				c.SurName,
			});

			return ApiResult.Success(new { Users = returnData });	
		}

		public ActionResult Single(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid argument to api");

			Course course = _courseService.GetById((int)id);

			if (course == null)
				return ApiResult.Fail("Course does not exist");

			string currentUserId = User.Identity.GetUserId();
			IList<string> currentUserRoles = GetRolesForUser(currentUserId);

			// Needs to be either a student that takes the course or any teacher
			if (currentUserRoles.Contains("Teacher") || _courseService.HasUserWithId(course, currentUserId))
			{
				var returnData = new
				{
					Course = new
					{
						course.Id,
						course.Name,
						course.Description
					}
				};
				return ApiResult.Success(returnData);
			}
			else
				return ApiResult.Fail("No access");
		}

	}
}
