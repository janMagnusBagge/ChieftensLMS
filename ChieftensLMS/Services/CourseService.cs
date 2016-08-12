using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChieftensLMS.Services
{
	public class CourseService
	{
		ApplicationDbContext _db = null;

		public CourseService(ApplicationDbContext context)
		{
			_db = context;
		}

		public List<Course> TestMethod()
		{
			return _db.Courses.ToList();
		}
	}
}