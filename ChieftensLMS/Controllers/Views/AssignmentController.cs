﻿using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using ChieftensLMS.Services;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Hosting;

namespace ChieftensLMS.Controllers
{
    public class AssignmentController : Controller
    {
		private ApplicationDbContext _context;
		private AssignmentService _AssignmentService;

		public AssignmentController()
		{
			_context = new ApplicationDbContext();
			_AssignmentService = new AssignmentService(_context, HostingEnvironment.MapPath("~\\Uploads\\TurnIns\\"));
		}
        // GET: Assignment
		public ActionResult Index(int? id)
        {
			//var assigment = GetAssigments(3);
			//return View(assigment);
			return View(id);
        }

		//TODO: are we gona put authorize attribute ?
		public ActionResult Details(int? id)
		{
			return View(id);
		}

		[Authorize(Roles = "Teacher")]
		public ActionResult Create(int? id)
		{
			return View(id);
		}

		[Authorize(Roles = "Teacher")]
		public ActionResult Edit(int? id)
		{
			return View(id);
		}

		//public ActionResult Delete(int? id)
		//{
		//	return View(id);
		//}

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