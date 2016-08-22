using ChieftensLMS.Classes;
using ChieftensLMS.Services;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ChieftensLMS.Controllers.Api
{
	public class CourseApiController : LMSController
	{
		private CourseService _courseService = LMSHelper.GetCourseService();

		// TODO: Ta bort? Ska alla kurser kunna visas?
		[Authorize(Roles = "Teacher")]
		public ActionResult All()
		{
			return ApiResult.Success(new { Courses = _courseService.GetAllCourses() });
		}

		//KLAR
		[Authorize]
		public ActionResult Mine()
		{
			var result = _courseService.GetCoursesForUser(_currentUserId);

			if (result == null)
				return ApiResult.Fail("");
			else
				return ApiResult.Success(new { Courses = result });
		}

		//KLAR
		[Authorize]
		public ActionResult AllUsers(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid argument to api");

			var result = _courseService.GetUsersWithRolesForCourse((int)id, _currentUserId);

			if (result == null)
				return ApiResult.Fail("");
			else
				return ApiResult.Success(new { Users = result });
		}

		//KLAR
		[Authorize]
		public ActionResult Single(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid argument to api");

			var result = _courseService.GetCourseById((int)id, _currentUserId);
			
			if (result == null)
				return ApiResult.Fail("");
			else
				return ApiResult.Success(result);
	
				
		}

		[Authorize(Roles ="Teacher" )]
		public ActionResult RemoveUserFromCourse(string userId, int? courseId)
		{
			if (userId == null || courseId == null)
				return ApiResult.Fail("Invalid argument to api");

			var result = _courseService.RemoveUserFromCourse((int)courseId, userId, _currentUserId);

			if (result == false)
				return ApiResult.Fail("");
			else
				return ApiResult.Success(new { UserId = userId });
		}

		[Authorize(Roles = "Teacher")]
		public ActionResult UsersNotInCourse(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid argument to api");

			var result = _courseService.GetUsersNotInCourse((int)id, _currentUserId);

			if (result == null)
				return ApiResult.Fail("");
			else
				return ApiResult.Success(new { Users = result });
		}

		[Authorize(Roles = "Teacher")]
		public ActionResult AddUser(int? courseId, string userId)
		{
			if (courseId == null || userId == null)
				return ApiResult.Fail("Invalid argument to api");

			var result = _courseService.AddUser(userId, (int)courseId, _currentUserId);

			if (result == false)
				return ApiResult.Fail("");
			else
				return ApiResult.Success(new { UserId = userId });
		}

		public ActionResult CreateCourse(string name, string description)
		{
			if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description))
				return ApiResult.Fail("");

			var result = _courseService.CreateCourse(name, description, _currentUserId);

			if (result == null)
				return ApiResult.Fail("");


			return ApiResult.Success(new { CourseId = result });
		}

		
	}
}
