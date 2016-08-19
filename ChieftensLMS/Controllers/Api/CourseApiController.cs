using ChieftensLMS.Classes;
using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using ChieftensLMS.Models.DTOModels;
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
		private StudentAccountService _accountService;

		public CourseApiController()
		{
			_context = new ApplicationDbContext();
			_courseService = new CourseService(_context);
		}

		private ApplicationUserManager GetUserManager()
		{
			return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
		}

		private string GetCurrentUserId()
		{
			return User.Identity.GetUserId();
		}

		// TODO: Ta bort? Ska alla kurser kunna visas?
		[Authorize(Roles = "Teacher")]
		public ActionResult All()
		{
			var coursesJsonData = _courseService.GetAll()
				.Select(course => new
				{
					course.Id,
					course.Name,
					course.Description
				});

			return ApiResult.Success(new { Courses = coursesJsonData });
		}

		//KLAR
		[Authorize]
		public ActionResult Mine()
		{
			var coursesJsonData = _courseService.GetForUserId(GetCurrentUserId())
						.Select(course =>
							new
							{
								Id = course.Id,
								Name = course.Name,
								Description = course.Description
							}
						);


			return ApiResult.Success(new { Courses = coursesJsonData });
		}

		//KLAR
		[Authorize]
		public ActionResult AllUsers(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid argument to api");

			Course course = _courseService.GetById((int)id);

			if (course == null)
				return ApiResult.Fail("Course does not exist");

			// Dont need projection here, its done in the method and returned as a DTO
			var courseUsersJsonData = _courseService.GetUsersForCourse(course.Id);

			return ApiResult.Success(new { Users = courseUsersJsonData });
		}

		//KLAR
		[Authorize]
		public ActionResult Single(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid argument to api");

			Course course = _courseService.GetById((int)id);

			if (course == null)
				return ApiResult.Fail("Course does not exist");

			// Needs to be either a student that takes the course or any teacher
			if (GetUserManager().IsInRole(GetCurrentUserId(), "Teacher") || _courseService.HasUserWithId(course.Id, GetCurrentUserId()))
			{
				var courseJsonData = new
				{
					Course = new
					{
						Id = course.Id,
						Name = course.Name,
						Description = course.Description
					}
				};
				return ApiResult.Success(courseJsonData);
			}
			else
				return ApiResult.Fail("No access");
		}
	}
}
