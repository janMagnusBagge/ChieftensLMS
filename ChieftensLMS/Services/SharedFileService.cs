using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace ChieftensLMS.Services
{
	public class SharedFileDTO
	{
		public string Name { get; set; }
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public int CourseId { get; set; }
		public string OwnerId { get; set; }
		public string FileName { get; set; }
	}

	public class SharedFileWithCourseDTO
	{
		public string Name { get; set; }
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public int CourseId { get; set; }
		public string CourseName { get; set; }
	}

	public class SharedFileWithOwnerDTO
	{
		public string Name { get; set; }
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public string OwnerId { get; set; }
		public string OwnerName { get; set; }
		public string OwnerSurname { get; set; }
	}

	public class SharedFileService
	{
		ApplicationDbContext _db;
		string _fileDirectory;

		public SharedFileService(ApplicationDbContext context, string fileDirectory)
		{
			_fileDirectory = fileDirectory;
			_db = context;
		}

		//KLAR
		public IEnumerable<SharedFileWithCourseDTO> GetSharedFilesWithCourseForUser(string userId)
		{
			if (isValidUser(userId) == false)
				return null;

			return _db.SharedFiles.Where(sharedFile => sharedFile.UserId == userId)
					   .Include(sharedFile => sharedFile.Course)
					   .Select(sf => new SharedFileWithCourseDTO()
					   {
						   Id = sf.Id,
						   CourseId = sf.CourseId,
						   CourseName = sf.Course.Name,
						   Date = sf.Date,
						   Name = sf.Name
					   })
					   .AsEnumerable();
		}

		// KLAR
		public IEnumerable<SharedFileWithOwnerDTO> GetSharedFilesWithOwnerForCourse(int courseId, string asUser)
		{
			if (IsInCourse(courseId, asUser) == false)
				return null;

			return _db.SharedFiles.Where(sharedFile => sharedFile.CourseId == courseId)
					  .Include(sharedFile => sharedFile.User)
					  .Select(sf => new SharedFileWithOwnerDTO()
					  {
						  Id = sf.Id,
						  Date = sf.Date,
						  Name = sf.Name,
						  OwnerId = sf.UserId,
						  OwnerName = sf.User.Name,
						  OwnerSurname = sf.User.SurName
					  })
					  .AsEnumerable();
		}

		public SharedFileDTO GetById(int sharedFileId, string asUser)
		{
			var sf = _db.SharedFiles.Find(sharedFileId);

			if (sf == null)
				return null;

			if ((IsInCourse(sf.CourseId, asUser) || sf.UserId == asUser) == false)
				return null;

			return new SharedFileDTO()
			{
				Id = sf.Id,
				CourseId = sf.CourseId,
				Name = sf.Name,
				Date = sf.Date,
				FileName = sf.FileName,
				OwnerId = sf.UserId
			};
		}

		public string GetPhysicalPath(int sharedFileId)
		{
			var sharedFile = _db.SharedFiles.Find(sharedFileId);

			if (sharedFile == null)
				return null;

			string filePath = Path.Combine(_fileDirectory, sharedFile.Id.ToString());
			if (File.Exists(filePath) == false)
				return null;

			return filePath;
		}

		public bool Delete(int sharedFileId, string asUser)
		{
			var sharedFile = _db.SharedFiles.Find(sharedFileId);

			if (sharedFile == null)
				return false;

			if ((IsTeacherForCourse(asUser, sharedFile.CourseId) || sharedFile.UserId == asUser) == false)
				return false;

			File.Delete(Path.Combine(_fileDirectory, sharedFile.Id.ToString()));
			_db.Entry(sharedFile).State = EntityState.Deleted;
			_db.SaveChanges();
			return true;
		}





		public bool isValidUser(string userId)
		{
			return _db.Users.Find(userId) != null;
		}

		public bool IsValidCourse(int courseId)
		{
			return _db.Courses.Find(courseId) != null;
		}

		// Checks if a user is in a course
		public bool IsInCourse(int courseId, string userId)
		{
			return _db.Courses.Any(c => c.Id == courseId && c.Users.Any(u => u.Id == userId));
		}

		// Checks if a user is in a role
		public bool IsTeacherForCourse(string userId, int courseId)
		{
			return _db.Roles.Any(r => r.Name == "Teacher" && r.Users.Any(u => u.UserId == userId) && _db.Courses.Any(c => c.Id == courseId && c.Users.Any(u => u.Id == userId)));
		}




		//public void DeleteSharedFileAsUser(SharedFile sharedFile, string userId)
		//{
		//	if (userId == sharedFile.UserId)
		//	{
		//		DeleteSharedFile(sharedFile);
		//	}
		//}



		public int? AddSharedFile(int courseId, string userId, string name, string fileName, Stream stream)
		{
			if (IsInCourse(courseId, userId) == false)
				return null;

			SharedFile newSharedFile = new SharedFile()
				{ Date = DateTime.Now, CourseId = courseId, UserId = userId, Name = name, FileName = fileName };

			_db.SharedFiles.Add(newSharedFile);
			_db.SaveChanges();

			string filePath = Path.Combine(_fileDirectory, newSharedFile.Id.ToString());
			using (var file = File.Create(filePath))
			{
				stream.Seek(0, SeekOrigin.Begin);
				stream.CopyTo(file);
			}

			
			return newSharedFile.Id;

		}

	}
}