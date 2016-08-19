using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using ChieftensLMS.Services;
using Microsoft.AspNet.Identity;
using System.Web.Hosting;
using ChieftensLMS.Classes;
using Microsoft.AspNet.Identity.Owin;

namespace ChieftensLMS.Controllers
{
	public class SharedFileApiController : Controller
	{
		private ApplicationDbContext _db;
		private SharedFileService _sharedFileService;
		private CourseService _courseService;

		public SharedFileApiController()
		{
			_db = new ApplicationDbContext();
			_sharedFileService = new SharedFileService(_db, HostingEnvironment.MapPath("~\\Uploads\\SharedFiles\\"));
			_courseService = new CourseService(_db);
		}

		public IList<string> GetRolesForUser(string id)
		{
			var _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			IList<string> currentUserRoles = _userManager.GetRoles(id);

			return currentUserRoles;
		}

		[Authorize]
		public ActionResult Mine()
		{
			string currentUserId = User.Identity.GetUserId();

			var returnData = _sharedFileService.GetForUser(currentUserId).Select(sf => new
			{
				sf.Name,
				sf.Id,
				sf.Date,
				sf.CourseId,
				CourseName = sf.Course.Name
			});

			return ApiResult.Success(new { SharedFiles = returnData });
		}

		[Authorize]
		public ActionResult ForCourse(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid request");

			Course course = _courseService.GetById((int)id);

			if (course == null)
				return ApiResult.Fail("Course does not exist");

			string currentUserId = User.Identity.GetUserId();
			IList<string> currentUserRoles = GetRolesForUser(currentUserId);

			// Needs to be either a student that takes the course or any teacher
			if (currentUserRoles.Contains("Teacher") || _courseService.HasUserWithId(course, currentUserId))
			{
				var sharedFiles = _sharedFileService.GetForCourse(course).Select(s => new {
					s.Id,
					s.Name,
					s.Date,
					Owner = (s.UserId == currentUserId) ? null : s.User.Name + " " + s.User.SurName
				});
				return ApiResult.Success(new { SharedFiles = sharedFiles });
			}
			else
			{
				return ApiResult.Fail("No access");
			}
		}

		[Authorize]
		public ActionResult Download(int? id)
		{
			string currentUserId = User.Identity.GetUserId();
			IList<string> currentUserRoles = GetRolesForUser(currentUserId);

			if (id == null)
				return ApiResult.Fail("Invalid request");

			SharedFile sharedFile = _sharedFileService.GetById((int)id);

			if (sharedFile == null)
				return ApiResult.Fail("Invalid file");
			
			if (currentUserRoles.Contains("Teacher") || _courseService.HasUserWithId(sharedFile.CourseId ,currentUserId) || sharedFile.UserId == currentUserId)
			{
				string physicalFileToReturn = _sharedFileService.GetPhysicalPath(sharedFile);

				if (physicalFileToReturn == null)
					return ApiResult.Fail("File deleted from server");

				string mimeType = MimeMapping.GetMimeMapping(sharedFile.FileName);

				return File(physicalFileToReturn, mimeType, sharedFile.FileName);
			}
			else
				return ApiResult.Fail("No Access");
		}

		[Authorize]
		public ActionResult Delete(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Bad request");

			var sharedFile = _sharedFileService.GetById((int)id);

			if (sharedFile == null)
				return ApiResult.Fail("Invalid file");

			// Permission checks
			string currentUserId = User.Identity.GetUserId();
			IList<string> currentUserRoles = GetRolesForUser(currentUserId);

			// Only delete if the file is owned by user or ANY teacher
			if (currentUserRoles.Contains("Teacher") || sharedFile.UserId == currentUserId)
			{
				_sharedFileService.Delete(sharedFile);
				return ApiResult.Success(new { Id = id });
			}
			else
				return ApiResult.Fail("No access to delete this shared file");
			

		}

	}
}
