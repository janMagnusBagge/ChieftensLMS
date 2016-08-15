using ChieftensLMS.Controllers;
using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChieftensLMS.Services
{
	public class AssignmentService
	{
		LMSDbContext _db = null;
		UnitOfWork _unitOfWork = null;

		public AssignmentService(LMSDbContext context)
		{
			_db = context;
			_unitOfWork = new UnitOfWork(_db);
		}

		internal IEnumerable<Assignment> GetAssignmentForCourse(int courseId)
		{
			return _unitOfWork.AssignmentRepository.Get(v => v.CourseId == courseId);
			//return _unitOfWork.CourseRepository.Get(v => v.Users.FirstOrDefault(o => o.Id == user.Id) != null);
		}

		internal Assignment GetAssignment(int Id)
		{
			return _unitOfWork.AssignmentRepository.GetById(Id);
		}
	}
}