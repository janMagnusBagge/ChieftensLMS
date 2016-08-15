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
	public class AssigmentApiController : Controller
	{
		private LMSDbContext _context;
		private AssignmentService _AssignmentService;

		public AssigmentApiController()
		{
			_context = new LMSDbContext();
			_AssignmentService = new AssignmentService(_context);
		}

		public ActionResult GetAssignmentForCourse(int courseId)
		{
			var assigmentsForCourse = _AssignmentService.GetAssignmentForCourse(courseId)
				.Select(a => new
				{
					a.Id,
					a.CourseId,
					a.Description,
					a.Date,
					a.ExpirationDate,
					a.Name
				}
				);
			var returnObject = new
			{
				data = new { assigments = assigmentsForCourse },
				success = true
			};
			return Json(returnObject, JsonRequestBehavior.AllowGet);
			//return Json(assigmentsForCourse, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetAssigment(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Hej");
			var assigment = _AssignmentService.GetAssignment((int)id);

			if (assigment == null)
				return ApiResult.Fail("Hej");

			var returnData = new
				{
					assignment = new
					{
						Id = assigment.Id,
						CourseId = assigment.CourseId,
						Description = assigment.Description ,
						Date = assigment.Date,
						ExpirationDate = assigment.ExpirationDate,
						Name = assigment.Name
					}
				};

			return ApiResult.Success(returnData);
		}
	}
}