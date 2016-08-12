﻿using ChieftensLMS.Controllers;
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

		public IEnumerable<Course> GetCoursesForUserId(string userId)
		{
			return _unitOfWork.CourseRepository.Get(v => v.Users.FirstOrDefault(o => o.Id == userId) != null);
		}

		public Course GetCourseById(int courseId)
		{
			return _unitOfWork.CourseRepository.GetById(courseId);
		}
	}
}