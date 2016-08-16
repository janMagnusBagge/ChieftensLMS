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

namespace ChieftensLMS.Controllers
{
    public class SharedFileController : Controller
    {
		private LMSDbContext _context;
		private SharedFileService _sharedFileService;

		public SharedFileController()
		{
			_context = new LMSDbContext();
			_sharedFileService = new SharedFileService(_context, HostingEnvironment.MapPath("~\\Uploads\\SharedFiles\\"));
		}

		// GET: SharedFiles
		public ActionResult ForCourse(int? id)
        {
			return View(id);
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
