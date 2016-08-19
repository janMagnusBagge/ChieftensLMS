using ChieftensLMS.Controllers;
using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChieftensLMS.Services
{
	public class CourseService
	{
		ApplicationDbContext _db;

		public CourseService(ApplicationDbContext context)
		{
			_db = context;
		}

		public IEnumerable<Course> GetForUserId(string userId)
		{
			return _db.Courses.Where(course => course.Users.Any(user => user.Id == userId));
		}

		public IEnumerable<Course> GetAll()
		{
			return _db.Courses.ToList();
		}
		
		public Course GetById(int courseId)
		{
			return _db.Courses.Find(courseId);
		}

		public IEnumerable<ApplicationUser> GetUsersForCourse(Course forCourse)
		{
			return _db.Users.Where(e => e.Courses.Any(f => f.Id == forCourse.Id));
		}

		public bool HasUserWithId(Course course, string userId)
		{
			return HasUserWithId(course.Id, userId);
		}

		public bool HasUserWithId(int courseId, string userId)
		{
			return _db.Courses.Any(course => course.Users.Any(user => user.Id == userId));
		}
	}
}