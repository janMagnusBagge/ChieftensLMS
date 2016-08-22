using ChieftensLMS.DAL;
using ChieftensLMS.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace ChieftensLMS.Classes
{
	public static class LMSHelper
	{
		public static ApplicationUserManager GetUserManager()
		{
			return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
		}

		public static string GetCurrectUserId()
		{
			return HttpContext.Current.GetOwinContext().Request.User.Identity.GetUserId();
		}

		public static ApplicationDbContext GetApplicationContext()
		{
			return HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
		}

		public static CourseService GetCourseService()
		{
			return new CourseService(GetApplicationContext());
		}

		public static SharedFileService GetSharedFileService()
		{
			return new SharedFileService(GetApplicationContext(), HostingEnvironment.MapPath("~\\Uploads\\SharedFiles\\"));
		}

		public static RoleManager<IdentityRole> GetRoleManager()
		{
			return new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(GetApplicationContext()));
		}
	}
}