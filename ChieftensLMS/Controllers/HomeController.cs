using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChieftensLMS.Controllers
{
	public class HomeController : Controller
	{
		UnitOfWork unitOfWork = new UnitOfWork(new ApplicationDbContext());



		public ActionResult Index()
		{
			ApplicationUser user = unitOfWork.ApplicationUser.Get(x => x.UserName == "Teacher@Teacher.com", null, "Courses.Lectures").First();

			return Json(user.Courses.First().Lectures.First().Name, JsonRequestBehavior.AllowGet);

			
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}