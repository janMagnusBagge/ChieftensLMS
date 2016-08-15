using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using ChieftensLMS.Services;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ChieftensLMS.Controllers
{
    public class AssignmentController : Controller
    {
		private LMSDbContext _context;
		private AssignmentService _AssignmentService;
		//private CourseService _courseService;
		//private ApplicationUserManager _userManager;

		public AssignmentController()
		{
			_context = new LMSDbContext();
			_AssignmentService = new AssignmentService(_context);
		}
        // GET: Assignment
        public ActionResult Index()
        {
			//var assigment = GetAssigments(3);
			//return View(assigment);
			return View();
        }

		//public IEnumerable<Assignment> GetAssigments(int courseId)
		//{
		//	//var user = _userManager.FindById(User.Identity.GetUserId());
		//	//var coursesForUser = _AssignmentService.GetCoursesForUser(user);

		//	var assigmentsForCourse = _AssignmentService.GetAssignmentForCourse(courseId);
		//	return assigmentsForCourse;
		//	//return Json(assigmentsForCourse, JsonRequestBehavior.AllowGet);
		//}
    }
}