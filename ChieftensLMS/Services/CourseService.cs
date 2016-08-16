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
		LMSDbContext _db = null;
		UnitOfWork _unitOfWork = null;

		public CourseService(LMSDbContext context)
		{
			_db = context;
			_unitOfWork = new UnitOfWork(_db);
		}

		public IEnumerable<Course> GetForUserId(string id)
		{
			return _unitOfWork.CourseRepository.Get(v => v.Users.FirstOrDefault(o => o.Id == id) != null);
		}

		public IEnumerable<Course> GetAll()
		{
			return _unitOfWork.CourseRepository.Get(v => true);
		}

		public Course GetById(int id)
		{
			return _unitOfWork.CourseRepository.GetById(id);
		}
	}
}