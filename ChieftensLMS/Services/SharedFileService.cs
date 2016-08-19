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
	public class SharedFileService
	{
		ApplicationDbContext _db;
		string _fileDirectory;

		public SharedFileService(ApplicationDbContext context, string fileDirectory)
		{
			_fileDirectory = fileDirectory;
			_db = context;
		}

		public IEnumerable<SharedFile> GetForUser(string userId)
		{
			return _db.SharedFiles.Where(sharedFile => sharedFile.UserId == userId).Include(sharedFile => sharedFile.Course);
		}

		public IEnumerable<SharedFile> GetForCourse(Course course)
		{
			return _db.SharedFiles.Where(sharedFile => sharedFile.CourseId == course.Id).Include(sharedFile => sharedFile.User);
		}

		public SharedFile GetById(int sharedFileId)
		{
			return _db.SharedFiles.Find(sharedFileId);
		}

		public string GetPhysicalPath(SharedFile sharedFile)
		{
			string filePath = Path.Combine(_fileDirectory, sharedFile.Id.ToString());

			if (File.Exists(filePath) == false)
				return null;

			return filePath;
		}

		public void Delete(SharedFile sharedFile)
		{
			File.Delete(Path.Combine(_fileDirectory, sharedFile.Id.ToString()));
			_db.Entry(sharedFile).State = EntityState.Deleted;
			_db.SaveChanges();
		}

		//public void DeleteSharedFileAsUser(SharedFile sharedFile, string userId)
		//{
		//	if (userId == sharedFile.UserId)
		//	{
		//		DeleteSharedFile(sharedFile);
		//	}
		//}



		//public void AddSharedFileAsUserId(int courseId, string userId, string name, string fileName, Stream stream)
		//{
		//	SharedFile newSharedFile = new SharedFile() { Date = DateTime.Now, CourseId = courseId, UserId = userId, Name = name, FileName = fileName };
		//	_unitOfWork.SharedFileRepository.Add(newSharedFile);
		//	_unitOfWork.Save();

		//	string filePath = Path.Combine(_fileDirectory, newSharedFile.Id.ToString());

		//	using (var file = File.Create(filePath))
		//	{
		//		stream.Seek(0, SeekOrigin.Begin);
		//		stream.CopyTo(file);
		//	}


		//}

	}
}