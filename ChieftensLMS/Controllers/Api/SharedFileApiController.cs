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
using ChieftensLMS.Models.DTOModels;

namespace ChieftensLMS.Controllers
{
	public class SharedFileApiController : Controller
	{
		private ApplicationDbContext _context;
		private SharedFileService _sharedFileService;
		private CourseService _courseService;

		public SharedFileApiController()
		{
			_context = new ApplicationDbContext();
			_sharedFileService = new SharedFileService(_context, HostingEnvironment.MapPath("~\\Uploads\\SharedFiles\\"));
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

		[Authorize]
		public ActionResult Mine()
		{

			var sharedFilesJsonData = _sharedFileService.GetForUser(GetCurrentUserId()).Select(sf => new
			{
				Name = sf.Name,
				Id = sf.Id,
				Date = sf.Date,
				CourseId = sf.CourseId,
				CourseName = sf.Course.Name
			});

			return ApiResult.Success(new { SharedFiles = sharedFilesJsonData });
		}

		[Authorize]
		public ActionResult ForCourse(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid request");

			Course course = _courseService.GetById((int)id);

			if (course == null)
				return ApiResult.Fail("Course does not exist");

			// Needs to be either a student that takes the course or any teacher
			if (GetUserManager().IsInRole(GetCurrentUserId(), "Teacher") || _courseService.HasUserWithId(course.Id, GetCurrentUserId()))
			{
				var sharedFiles = _sharedFileService.GetForCourse(course.Id).Select(s => new {
					Id = s.Id,
					Name = s.Name,
					Date = s.Date,
					Owner = (s.UserId == GetCurrentUserId()) ? null : s.User.Name + " " + s.User.SurName
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
			if (id == null)
				return ApiResult.Fail("Invalid request");

			SharedFile sharedFile = _sharedFileService.GetById((int)id);

			if (sharedFile == null)
				return ApiResult.Fail("Invalid file");
			
			if (GetUserManager().IsInRole(GetCurrentUserId(), "Teacher") || _courseService.HasUserWithId(sharedFile.CourseId , GetCurrentUserId()) || sharedFile.UserId == GetCurrentUserId())
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

			// Only delete if the file is owned by user or ANY teacher
			if (GetUserManager().IsInRole(GetCurrentUserId(), "Teacher") || sharedFile.UserId == GetCurrentUserId())
			{
				_sharedFileService.Delete(sharedFile);
				return ApiResult.Success(new { Id = id });
			}
			else
				return ApiResult.Fail("No access to delete this shared file");
			

		}

	}
}
