using ChieftensLMS.Classes;
using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using ChieftensLMS.Models.DTOModels;
using ChieftensLMS.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace ChieftensLMS.Controllers.Api
{
	public class CourseManageApiController : Controller
	{
		private ApplicationDbContext _context;
		private CourseService _courseService;
		private StudentAccountService _accountService;

		public CourseManageApiController()
		{
			_context = new ApplicationDbContext();
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

		[Authorize(Roles = "Teacher")]
		public ActionResult AllStudents()
		{
			return null;
		}
		
	}
}
