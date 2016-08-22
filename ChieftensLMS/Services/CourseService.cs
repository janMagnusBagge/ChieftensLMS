using ChieftensLMS.Controllers;
using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using ChieftensLMS.Models.DTOModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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

		// KLAR
		public IEnumerable<Course> GetForUserId(string userId)
		{
			return _db.Courses.Where(course => course.Users.Any(user => user.Id == userId))
				.ToList();
		}

		// KLAR
		public IEnumerable<Course> GetAll()
		{
			return _db.Courses.ToList();
		}

		// KLAR
		public Course GetById(int courseId)
		{
			var course = _db.Courses.Find(courseId);

			if (course == null)
				return null;
			else
				return course;
		}

		//KLAR
		public IEnumerable<UserRolesDTO> GetUsersForCourse(int courseId)
		{
			return _db.Users.Where(user => user.Courses.Any(course => course.Id == courseId))
				.Select(e => new UserRolesDTO()
				{
					Name = e.Name,
					SurName = e.SurName,
					Roles = _db.Roles.Where(x => x.Users.Any(v => v.UserId == e.Id))
							.Select(r => r.Name).ToList()
				}).ToList();
		}

		//KLAR
		public bool HasUserWithId(int courseId, string userId)
		{
			return _db.Courses.Any(course => course.Users.Any(user => user.Id == userId));
		}
	}
}