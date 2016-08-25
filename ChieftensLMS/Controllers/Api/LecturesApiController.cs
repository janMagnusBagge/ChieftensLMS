using ChieftensLMS.Classes;
using ChieftensLMS.DAL;
using ChieftensLMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

		
		public ActionResult GetLecturesForUser(string userId)
		{
			if (userId == null)
				return ApiResult.Fail("Invalid argument to api");

			var result = _LecturesService.GetLecturesForUser(userId);

			if (result == null)
				return ApiResult.Fail("");
			else
				return ApiResult.Success(new { Lectures = result });
		}
    }
}