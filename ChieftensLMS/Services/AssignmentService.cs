using ChieftensLMS.Controllers;
using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace ChieftensLMS.Services
{
	public class AssignmentService
	{
		ApplicationDbContext _db = null;
		UnitOfWork _unitOfWork = null;
		string _fileDirectory = null;

		/*
		 * Initiate the service with the sent in data
		 */
		public AssignmentService(ApplicationDbContext context, string fileDirectory)
		{
			_fileDirectory = fileDirectory;
			_db = context;
			_unitOfWork = new UnitOfWork(_db);
		}

		/*
		 * Returns Assignments for specified course.
		 */
		internal IEnumerable<Assignment> GetAssignmentForCourse(int courseId)
		{
			//return _unitOfWork.AssignmentRepository.Get(v => v.CourseId == courseId);
			return _db.Assignments.Where(v => v.CourseId == courseId);
		}

		/*
		 * Return specific assignment based on sent in Id
		 */
		internal Assignment GetAssignment(int Id)
		{
			//return _unitOfWork.AssignmentRepository.GetById(Id);
			return _db.Assignments.Find(Id);
		}
		/*
		 * Returns TurnIn / Assignment files based on sent in Id.
		 */
		internal IEnumerable<TurnIn> GetFilesForAssignmentById(int assignmentId)
		{
			//return _unitOfWork.TurnInRepository.Get
			//	(
			//		v => v.AssignmentId == assignmentId,
			//		sa => sa.OrderBy(x => x.Date),
			//		"User"
			//	);
			return _db.TurnIns.Where(turnIn => turnIn.AssignmentId == assignmentId)
							  .Include(trunIn => trunIn.User)
							  .OrderBy(x => x.Date);
		}

		/*
		 * Returns a specifik Turnin file based on sent in Id
		 */
		public TurnIn GetTurnInById(int id)
		{
			//return _unitOfWork.TurnInRepository.GetById(id);
			return _db.TurnIns.Find(id);
		}

		/*
		 * Delete the specifik turnin file.
		 */
		//TODO: if there should be another path then the privously chosen 
		public void DeleteAssignmentFile(TurnIn turnInFile)
		{
			File.Delete(Path.Combine(_fileDirectory, turnInFile.Id.ToString()));
			_db.Entry(turnInFile).State = EntityState.Deleted;
			_db.SaveChanges();
			//_unitOfWork.TurnInRepository.Delete(turnInFile.Id);
			//_unitOfWork.Save();
		}

		/*
		 * Create assignment based on sent in data.
		 */
		//TODO: Throw exception if could not save
		public bool CreateAssignment(int courseId, string name, string description, DateTime date)
		{
			Assignment assignmentToCreate = new Assignment();
			assignmentToCreate.CourseId = courseId;
			assignmentToCreate.Name = name;
			assignmentToCreate.Description = description;
			assignmentToCreate.ExpirationDate = date;
			assignmentToCreate.Date = DateTime.Now;
			//_unitOfWork.AssignmentRepository.Add(assignmentToCreate);
			//_unitOfWork.Save();
			try
			{
				_db.Assignments.Add(assignmentToCreate);
				_db.SaveChanges();
			}
			catch (Exception ec)
			{
				Console.WriteLine(ec.Message);
				return false;
			}

			return true;
		}

		/*
		 * Uppdates assignment based on sent in data anad send to the method/funktion that takes assignment
		 */
		//TODO: Throw exception if could not save
		public bool UpdateAssignment(int id, string name, string description, DateTime date)
		{
			Assignment assignmentToUpdate = GetAssignment(id);
			
			assignmentToUpdate.Name = name;
			assignmentToUpdate.Description = description;
			assignmentToUpdate.ExpirationDate = date;

			return UpdateAssignment(assignmentToUpdate);
			
		}
		/*
		 * Uppdates the specifik assignment
		 */
		//TODO: Throw exception if could not save
		public bool UpdateAssignment(Assignment assignmentToUpdate)
		{
			try
			{
				//_unitOfWork.AssignmentRepository.Update(assignmentToUpdate);
				_db.Assignments.Attach(assignmentToUpdate);
				_db.Entry(assignmentToUpdate).State = EntityState.Modified;
				_db.SaveChanges();
				//_db.Assignments.Entry(assignmentToUpdate).State = EntityState.Modified;
				//_unitOfWork.Save();
			}
			catch (Exception ec)
			{
				Console.WriteLine(ec.Message);
				return false;
			}
			return true;
		}

		/*
		 * Returns a path to where the turnIn is lokated 
		 */
		public string GetPhysicalPath(int turnInFileId)
		{
			var turnInFile = _db.TurnIns.Find(turnInFileId);

			if (turnInFile == null)
				return null;

			string filePath = Path.Combine(_fileDirectory, turnInFile.Id.ToString());
			if (File.Exists(filePath) == false)
				return null;

			return filePath;
		}


		public int? AddTurnIn(int assignmentId, string userId, string name, string fileName, Stream stream)
		{
			if (UserHasAssignment(assignmentId, userId) == false)
				return null;

			TurnIn newTurnIn = new TurnIn()
				{ Date = DateTime.Now, AssignmentId = assignmentId, UserId = userId, Name = name, FileName = fileName };

			_db.TurnIns.Add(newTurnIn);
			_db.SaveChanges();

			string filePath = Path.Combine(_fileDirectory, newTurnIn.Id.ToString());
			using (var file = File.Create(filePath))
			{
				stream.Seek(0, SeekOrigin.Begin);
				stream.CopyTo(file);
			}


			return newTurnIn.Id;

		}

		public bool UserHasAssignment(int assignmentId, string userId)
		{
			return _db.Assignments.Any(e => e.Id == assignmentId && e.Course.Users.Any(r => r.Id == userId));
		}
	}
}