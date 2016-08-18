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
		public ActionResult ForCourse(int? id)
        {
			return View(id);
        }

		public ActionResult Mine()
		{
			return View();
		}
	}
}
