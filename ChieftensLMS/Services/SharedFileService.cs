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
		LMSDbContext _db = null;
		UnitOfWork _unitOfWork = null;
		string _fileDirectory = null;

		public SharedFileService(LMSDbContext context, string fileDirectory)
		{
			_fileDirectory = fileDirectory;
			_db = context;
			_unitOfWork = new UnitOfWork(_db);
		}

		public IEnumerable<SharedFile> GetSharedFilesForCourseById(int courseId)
		{
			return _unitOfWork.SharedFileRepository.Get
				(
					v => v.CourseId == courseId,
					sa => sa.OrderBy(x => x.Date),
					"User"
				);
		}

		public SharedFile GetSharedFileById(int id)
		{
			return _unitOfWork.SharedFileRepository.GetById(id);
		}

		public SharedFile GetSharedFileAsUserId(string userId, int fileId)
		{
			//Get the sharedfile with specified filedId, where the userId is in the courses of that file
			var sharedFile = _unitOfWork.SharedFileRepository.Get(
				v => (v.ID == fileId) && v.Course.Users.FirstOrDefault(e => e.Id == userId) != null)
				.FirstOrDefault();

			return sharedFile;
		}

		public FileStream GetPhysicalFile(SharedFile sharedFile)
		{
			string filePath = Path.Combine(_fileDirectory, sharedFile.ID.ToString());

			if (File.Exists(filePath) == false)
				return null;

			FileStream physicalFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: false);
			physicalFile.Seek(0, SeekOrigin.Begin);
			return physicalFile;
		}

		public String GetPhysicalFilePath(SharedFile sharedFile)
		{
			string filePath = Path.Combine(_fileDirectory, sharedFile.ID.ToString());

			if (File.Exists(filePath) == false)
				return null;

			return filePath;
		}

		public void DeleteSharedFileAsUser(SharedFile sharedFile, string userId)
		{
			if (userId == sharedFile.UserId)
			{
				DeleteSharedFile(sharedFile);
			}
		}

		public void DeleteSharedFile(SharedFile sharedFile)
		{
			File.Delete(Path.Combine(_fileDirectory, sharedFile.ID.ToString()));
			_unitOfWork.SharedFileRepository.Delete(sharedFile.ID);
			_unitOfWork.Save();
		}

		public void AddSharedFileAsUserId(int courseId, string userId, string name, string fileName, Stream stream)
		{
			SharedFile newSharedFile = new SharedFile() { Date = DateTime.Now, CourseId = courseId, UserId = userId, Name = name, FileName = fileName };
			_unitOfWork.SharedFileRepository.Add(newSharedFile);
			_unitOfWork.Save();

			string filePath = Path.Combine(_fileDirectory, newSharedFile.ID.ToString());

			using (var file = File.Create(filePath))
			{
				stream.Seek(0, SeekOrigin.Begin);
				stream.CopyTo(file);
			}


		}

	}
}