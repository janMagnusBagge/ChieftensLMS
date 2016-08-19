﻿using ChieftensLMS.Classes;
using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using ChieftensLMS.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace ChieftensLMS.Controllers.Api
{
	public class AssigmentApiController : Controller
	{
		private ApplicationDbContext _context;
		private AssignmentService _AssignmentService;

		public AssigmentApiController()
		{
			_context = new ApplicationDbContext();
			_AssignmentService = new AssignmentService(_context, HostingEnvironment.MapPath("~\\Uploads\\TurnIns\\"));
		}

		//TODO: Auth check needed
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
			
			return ApiResult.Success(new { assigments = assigmentsForCourse });
			//return Json(assigmentsForCourse, JsonRequestBehavior.AllowGet);
		}

		//TODO: Auth check needed
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

		//TODO: Auth check and if teacher is for course
		// Needs checking if the user has access to this assignment
		// Right now as long as you are a teacher you can see all the turnins for the course. Should it be changed to only teacher for course ?
		public ActionResult FilesForAssignment(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid request");

			if (_AssignmentService.GetAssignment((int)id) == null)
				return ApiResult.Fail("Invalid assignment");

			//var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			//var role = userManager.GetRoles(User.Identity.GetUserId());
			bool isTeacher = CheckTeacher();//role.Contains("Teacher");
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

		// Deletes a turnIn file by id
		// TODO: Check that the user has permission to delete that file (he/she needs to be the owner of it)
		public ActionResult DeleteTurnIn(int? id)
		{
			TurnIn turnIn = null;

			if (id == null)
				return ApiResult.Fail("Bad request");

			turnIn = _AssignmentService.GetTurnInById((int)id);

			if (turnIn != null)
			{
				_AssignmentService.DeleteAssignmentFile(turnIn);
				return ApiResult.Success(new { Id = id });
			}
			else
			{
				return ApiResult.Fail("Files doesnt exist");
			}

		}

		//TODO: Move this out to it's own place so all can pages can use it instead off only that use this ApiController
		public ActionResult CheckIfTeacher()
		{
			//var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			//var role = userManager.GetRoles(User.Identity.GetUserId());
			bool isTeacher = CheckTeacher();//role.Contains("Teacher");

			return ApiResult.Success(new { isTeacher = isTeacher });
		}

		//TODO: Move this out to it's own place so all can pages can use it instead off only that use this ApiController
		private bool CheckTeacher()
		{
			var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			var role = userManager.GetRoles(User.Identity.GetUserId());
			return role.Contains("Teacher");

		}

		//TODO: Fix so it throw exception and check if it did or not and return false if did
		public ActionResult CreateAssignment(int courseId, string name, string description, DateTime date)
		{
			_AssignmentService.CreateAssignment(courseId, name, description, date);
			bool ifCreated = true;
			return ApiResult.Success(new { ifCreated = ifCreated });
		}

		//TODO: Fix so it throw exception and check if it did or not and return false if did
		//TODO: Fix so can send in whole asignment
		public ActionResult UpdateAssignment(int id, string name, string description, DateTime date)
		{

			bool ifUpdated = _AssignmentService.UpdateAssignment(id, name, description, date); ;
			return ApiResult.Success(new { ifUpdated = ifUpdated });
		}

		//public ActionResult UpdateAssignment(Assignment assignmentToUpdate)
		//{
		//	_AssignmentService.UpdateAssignment(assignmentToUpdate);
		//	bool ifUpdated = true;
		//	return ApiResult.Success(new { ifUpdated = ifUpdated });
		//}
	}
}