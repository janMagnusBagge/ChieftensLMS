using ChieftensLMS.Controllers;
using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ChieftensLMS.Services
{
	public class AssignmentService
	{
		LMSDbContext _db = null;
		UnitOfWork _unitOfWork = null;
		string _fileDirectory = null;

		public AssignmentService(LMSDbContext context, string fileDirectory)
		{
			_fileDirectory = fileDirectory;
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

		internal IEnumerable<TurnIn> GetFilesForAssignmentById(int assignmentId)
		{
			return _unitOfWork.TurnInRepository.Get
				(
					v => v.AssignmentId == assignmentId,
					sa => sa.OrderBy(x => x.Date),
					"User"
				);
		}

		public TurnIn GetTurnInById(int id)
		{
			return _unitOfWork.TurnInRepository.GetById(id);
		}

		//TODO: if there should be another path then the privously chosen 
		public void DeleteAssignmentFile(TurnIn turnInFile)
		{
			File.Delete(Path.Combine(_fileDirectory, turnInFile.Id.ToString()));
			_unitOfWork.TurnInRepository.Delete(turnInFile.Id);
			_unitOfWork.Save();
		}
	}
}