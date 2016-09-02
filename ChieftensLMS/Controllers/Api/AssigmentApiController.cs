using ChieftensLMS.Classes;
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
	[Authorize]
	public class AssigmentApiController : Controller
	{
		private ApplicationDbContext _context;
		private AssignmentService _AssignmentService;
		/*
		 * Initiate the Assignment api
		 */
		public AssigmentApiController()
		{
			_context = new ApplicationDbContext();
			_AssignmentService = new AssignmentService(_context, HostingEnvironment.MapPath("~\\Uploads\\TurnIns\\"));
		}

		/*
		 * Returns Assignments based on course Id.
		 */
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
		}

		/*
		 * Returns a specifik assignment based on the sent in Id.
		 * Only returns one if all checks whent thrue otherwise returns an error.
		 */
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

		/*
		 * Returns Files that is for the specific assignment.
		 * Have some checks and if they dont go thrue returns information based on that.
		 */

		//TODO: Needs checking if the user has access to this assignment
		//		Right now as long as you are a teacher you can see all the turnins for the course. Should it be changed to only teacher for course ?
		public ActionResult FilesForAssignment(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid request");

			if (_AssignmentService.GetAssignment((int)id) == null)
				return ApiResult.Fail("Invalid assignment");


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
							Owner = default(string)
						}

					);

				return ApiResult.Success(new { assignmentFiles = assignmentFiles });
			}
			
		}

		/*
		 * Deletes specifide turnIn.
		 */
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

		/*
		 * Returns if the user is in Teacher role. Based on how the account is set upp they can be in teacher and student role at the same time.
		 */
		//TODO: Move this out to it's own place so all can pages can use it instead off only that use this ApiController
		//TODO: When moved this to its own place change all the calls to this to the new place. Some places need to check is Assignment and Lecture html files and maby some other places.
		public ActionResult CheckIfTeacher()
		{
			bool isTeacher = CheckTeacher();

			return ApiResult.Success(new { isTeacher = isTeacher });
		}

		/*
		 * Checks the current logged in account if it is in teacher role. Based on how the account is set upp they can be in teacher and student role at the same time.
		 */
		//TODO: Move this out to it's own place so all can pages can use it instead off only that use this ApiController
		private bool CheckTeacher()
		{
			var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			var role = userManager.GetRoles(User.Identity.GetUserId());
			return role.Contains("Teacher");

		}

		/*
		 * Creates a Assignment for a course.
		 */
		//TODO: Fix so it throw exception and check if it did or not and return false if did
		public ActionResult CreateAssignment(int courseId, string name, string description, DateTime date)
		{
			bool ifCreated = _AssignmentService.CreateAssignment(courseId, name, description, date);
			return ApiResult.Success(new { ifCreated = ifCreated });
		}

		/*
		 * Updates specifik assignment with the sent in information.
		 */
		//TODO: Fix so it throw exception and check if it did or not and return false if did
		//TODO: Fix so can send in whole asignment
		public ActionResult UpdateAssignment(int id, string name, string description, DateTime date)
		{

			bool ifUpdated = _AssignmentService.UpdateAssignment(id, name, description, date); ;
			return ApiResult.Success(new { ifUpdated = ifUpdated });
		}

		/*
		 * Download a turnin file. Based on the sent in Id.
		 * Has some checks to see if valid 
		 */
		[Authorize]
		public ActionResult Download(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid request");

			var result = _AssignmentService.GetTurnInById((int)id);

			if (result == null)
				return ApiResult.Fail("");

			string physicalFileToReturn = _AssignmentService.GetPhysicalPath(result.Id); //gets the physical path of the file so can download the file.
			if (physicalFileToReturn == null)
				return ApiResult.Fail("");

			string mimeType = MimeMapping.GetMimeMapping(result.FileName);

			return File(physicalFileToReturn, mimeType, result.FileName);
		}
	}
}