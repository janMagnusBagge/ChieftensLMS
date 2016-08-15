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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace ChieftensLMS.Controllers
{
    public class CourseController : Controller
    {
		private LMSDbContext _context;
		private CourseService _courseService;

		public CourseController()
		{
			_context = new LMSDbContext();
			_courseService = new CourseService(_context);
		}

        // GET: Courses
        public ActionResult Index()
		{ 
			return View();
        }

		// GET: Courses/Details/5
		public ActionResult Details(int? id)
		{
			return View(id);
		}

	}
}
