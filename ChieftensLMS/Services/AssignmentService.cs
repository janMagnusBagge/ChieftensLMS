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
		ApplicationDbContext _db = null;
		UnitOfWork _unitOfWork = null;

		public AssignmentService(ApplicationDbContext context)
		{
			_db = context;
			_unitOfWork = new UnitOfWork(_db);
		}

		internal IEnumerable<Assignment> GetAssignmentForCourse(int courseId)
		{
			return _unitOfWork.AssignmentRepository.Get(v => v.ID == courseId);
			//return _unitOfWork.CourseRepository.Get(v => v.Users.FirstOrDefault(o => o.Id == user.Id) != null);
		}
	}
}