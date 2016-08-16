using ChieftensLMS.Classes;
using ChieftensLMS.DAL;
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
	public class AssigmentApiController : Controller
	{
		private LMSDbContext _context;
		private AssignmentService _AssignmentService;

		public AssigmentApiController()
		{
			_context = new LMSDbContext();
			_AssignmentService = new AssignmentService(_context);
		}

		//Auth check needed
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

		//Auth check needed
		public ActionResult GetAssigment(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid request");
			var assigment = _AssignmentService.GetAssignment((int)id);

			if (assigment == null)
				return ApiResult.Fail("Invalid assignment");

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

		// Needs checking if the user has access to this course
		public ActionResult FilesForAssignment(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid request");

			if (_AssignmentService.GetAssignment((int)id) == null)
				return ApiResult.Fail("Invalid assignment");

			var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			var role = userManager.GetRoles(User.Identity.GetUserId());//.Single();
			bool isTeacher = role.Contains("Teacher");
			//if (role == "Teacher")
			//if (Roles.IsUserInRole("Teacher"))
			//if (User.IsInRole("Teacher"))
			if (isTeacher)
			{
				//Get all Assignment files for Assignment and project them to a new model
				var assignmentFiles = _AssignmentService.GetFilesForAssignmentById((int)id)
					.Select(i =>
						new
						{
							i.Id,
							i.Name,
							i.Date,
							Owner = (User.Identity.GetUserId() == i.UserId) ? null : i.User.Name + " " + i.User.SurName
						}

					);

				return ApiResult.Success(new { assignmentFiles = assignmentFiles });
				
			}
			else
			{
				//Get all Assignment files for Assignment and project them to a new model
				var assignmentFiles = _AssignmentService.GetFilesForAssignmentById((int)id)
					.Where(i => i.UserId == User.Identity.GetUserId())
					.Select(i =>
						new
						{
							i.Id,
							i.Name,
							i.Date,
							Owner = default(string)//(User.Identity.GetUserId() == i.UserId) ? null : i.User.Name + " " + i.User.SurName 
						}

					);

				return ApiResult.Success(new { assignmentFiles = assignmentFiles });
			}
			
		}
	}
}