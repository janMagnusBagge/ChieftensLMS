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
using System.IO;
using System.Web.Hosting;
using ChieftensLMS.Classes;

namespace ChieftensLMS.Controllers
{
	public class SharedFileApiController : Controller
	{
		private LMSDbContext _context;
		private SharedFileService _sharedFileService;
		private CourseService _courseService;

		public SharedFileApiController()
		{
			_context = new LMSDbContext();
			_sharedFileService = new SharedFileService(_context, HostingEnvironment.MapPath("~\\Uploads\\SharedFiles\\"));
			_courseService = new CourseService(_context);
		}

		// Needs checking if the user has access to this course
		public ActionResult ForCourse(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid request");

			if (_courseService.GetCourseById((int)id) == null)
				return ApiResult.Fail("Invalid course");

			//Get all shared files for course and project them to a new model
			var sharedFiles = _sharedFileService.GetSharedFilesForCourseById((int)id)
				.Select(i =>
					new
					{
						i.Id,
						i.Name,
						i.Date,
						Owner = (User.Identity.GetUserId() == i.UserId) ? null : i.User.Name + " " + i.User.SurName
					}

				);

			return ApiResult.Success(new { sharedFiles = sharedFiles });
		}

		public ActionResult Download(int? id)
		{
			String physicalFileToReturn = null;
			SharedFile sharedFile = null;

			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			sharedFile = _sharedFileService.GetSharedFileAsUserId(User.Identity.GetUserId(), (int)id);

			if (sharedFile == null)
				return Content("Id invalid or no access");

			physicalFileToReturn = _sharedFileService.GetPhysicalFilePath(sharedFile);

			if (physicalFileToReturn == null)
				return Content("File doesnt exist");

			string mimeType = MimeMapping.GetMimeMapping(sharedFile.FileName);

			return File(physicalFileToReturn, mimeType, sharedFile.FileName);
			//return File(_sharedFileService.GetPhysicalFile(sharedFile), mimeType, sharedFile.FileName);
		}

		public ActionResult Delete(int? id)
		{
			SharedFile sharedFile = null;

			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			sharedFile = _sharedFileService.GetSharedFileAsUserId(User.Identity.GetUserId(), (int)id);

			if (sharedFile != null)
				_sharedFileService.DeleteSharedFileAsUser(sharedFile, User.Identity.GetUserId());


			return null;
		}

		[HttpPost]
		public ActionResult Add(HttpPostedFileBase file, SharedFile sharedFile)
		{
			if (file != null && file.ContentLength > 0)
			{

				_sharedFileService.AddSharedFileAsUserId(sharedFile.CourseId, User.Identity.GetUserId(), sharedFile.Name, file.FileName, file.InputStream);
			}

			return Content("hej");
		}

		public ActionResult Add()
		{
			return View();
		}


	}
}
