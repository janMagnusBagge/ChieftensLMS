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
        public ActionResult Index()
		{ 
			return View();
        }

		public ActionResult Details(int? id)
		{
			return View(id);
		}

	}
}
