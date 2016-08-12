using ChieftensLMS.Controllers;
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
		UnitOfWork _unitOfWork = null;

		public CourseService(ApplicationDbContext context)
		{
			_db = context;
			_unitOfWork = new UnitOfWork(_db);
		}

		internal IEnumerable<Course> GetCoursesForUser(ApplicationUser user)
		{
			return _unitOfWork.CourseRepository.Get(v => v.Users.FirstOrDefault(o => o.Id == user.Id) != null);
		}
	}
}