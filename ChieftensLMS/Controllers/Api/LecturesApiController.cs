using ChieftensLMS.Classes;
using ChieftensLMS.DAL;
using ChieftensLMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ChieftensLMS.Models;

namespace ChieftensLMS.Controllers.Api
{
	[Authorize]
    public class LecturesApiController : Controller
    {
		private ApplicationDbContext _context;
		private LecturesService _LecturesService;

		/*
		 * Initiats the lecture service
		 */
		public LecturesApiController()
		{
			_context = new ApplicationDbContext();
			_LecturesService = new LecturesService(_context);
		}
		/*
		 * Gets lectures specified course
		 */
		public ActionResult GetLecturesForCourse(int? courseId)
		{
			if (courseId == null)
				return ApiResult.Fail("Invalid argument to api");

			var result = _LecturesService.GetLecturesForCourse((int)courseId);

			if (result == null)
				return ApiResult.Fail("");
			else
				return ApiResult.Success(new { Lectures = result });
		}

		/*
		 * Gets lectures specified user
		 */
		public ActionResult GetLecturesForUser(string id)
		{
			var userId = (id != null ? (id.Trim() != "" ? id : User.Identity.GetUserId()) : User.Identity.GetUserId());//User.Identity.GetUserId();
			if (userId == null)
				return ApiResult.Fail("Invalid argument to api");

			var result = _LecturesService.GetLecturesForUser(userId);

			if (result == null)
				return ApiResult.Fail("");
			else
				return ApiResult.Success(new { Lectures = result });
		}
		/*
		 * gets specified lecture
		 */
		public ActionResult GetLecture(int? lectureId)
		{
			if (lectureId == null)
				return ApiResult.Fail("Invalid argument to api");

			var result = _LecturesService.GetLecture((int)lectureId);

			if (result == null)
				return ApiResult.Fail("");
			else
				return ApiResult.Success(new { Lecture = result });

		}

		/*
		 * Updates specified lecture
		 */
		//TODO: Fix so it throw exception and check if it did or not and return false if did
		//TODO: Fix so can send in whole lecture
		public ActionResult UpdateLecture(int id, string name, string description, DateTime date, int timeInMin, DateTime startTime )
		{
			bool ifUpdated = _LecturesService.UpdateLecture(id, name, description, date, timeInMin, startTime);
			return ApiResult.Success(new { ifUpdated = ifUpdated });
		}

		/*
		 * Creates lecture with the sent in data.
		 */
		public ActionResult CreateLecture(int courseId, string name, string description, int timeInMin, int frequency, DateTime startdate, DateTime enddate, DateTime starttime)
		{
			bool ifCreated = _LecturesService.CreateLecture(courseId, name, description, timeInMin, frequency, startdate, enddate, starttime);
			return ApiResult.Success(new { ifCreated = ifCreated });
		}

		/*
		 * Gets the course name based on the sent in Id
		 */
		//TODO Move this to courseApi
		public ActionResult GetCourseName(int courseId)
		{

			string courseName = _LecturesService.GetCourseName(courseId);
			return ApiResult.Success(new { courseName = courseName });
		}

		/*
		 * Delete the specified lecture. Returns result of the checks if failed em or the Id of the deleted.
		 */
		public ActionResult DeleteLecture(int? lectureId)
		{
			Lecture lecture = null;
			if (lectureId == null)
				return ApiResult.Fail("Bad request");
			lecture = _LecturesService.GetLectureFromDb((int)lectureId);

			if (lecture != null)
			{
				bool ifDeleted = _LecturesService.DeleteLecture(lecture);
				if(ifDeleted == true)
					return ApiResult.Success(new { Id = lectureId });
				else
					return ApiResult.Fail("Could not delete");
			}
			else
			{
				return ApiResult.Fail("Files doesnt exist");
			}
		}


	}
}