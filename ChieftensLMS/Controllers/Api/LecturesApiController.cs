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

namespace ChieftensLMS.Controllers.Api
{
	[Authorize]
    public class LecturesApiController : Controller
    {
		private ApplicationDbContext _context;
		private LecturesService _LecturesService;

		public LecturesApiController()
		{
			_context = new ApplicationDbContext();
			_LecturesService = new LecturesService(_context);
		}

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
    }
}