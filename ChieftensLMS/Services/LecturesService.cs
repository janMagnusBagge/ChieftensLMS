using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ChieftensLMS.Services
{
	#region DTO Classes
	public class LectureDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime Date { get; set; }
		public string Description { get; set; }
		public int CourseId { get; set; }
		public int TimeInMin { get; set; }
		public string CourseName { get; set; }
		//public TimeSpan StartTime { get; set; }
		public string StartTime { get; set; }
		public int Hour { get; set; }
		public int Minute { get; set; }

		public LectureDTO()
		{
			
		}

		public LectureDTO(Lecture lecture)
		{
			Id = lecture.Id;
			Name = lecture.Name;
			Date = lecture.Date;
			Description = lecture.Description;
			CourseId = lecture.CourseId;
			TimeInMin = lecture.TimeInMin;
			CourseName = lecture.CourseId.ToString();
			//StartTime = new TimeSpan(Date.Hour,Date.Minute,Date.Second);
			//StartTime = Date.Hour+":"+ Date.Minute+":"+ Date.Second;
			StartTime = Date.ToString("hh:mm", CultureInfo.CurrentCulture);
			Hour = Date.Hour;
			Minute = Date.Minute;
		}
	}
	#endregion

	public class LecturesService
	{
		ApplicationDbContext _db;

		public LecturesService(ApplicationDbContext context)
		{
			_db = context;
		}
		/*
		 * Returns a List of lectures for specified course.
		 */
		public IEnumerable<LectureDTO> GetLecturesForCourse(int courseId)
		{
			if (IsValidCourse(courseId) == false)
				return null;
			Course course = _db.Courses.Find(courseId);
			string courseName = (course!=null)?course.Name:"";

			return _db.Lectures.Where(lecture => lecture.CourseId == courseId)
				.Select(lecture => new LectureDTO
				{
					Description = lecture.Description,
					Id = lecture.Id,
					Name = lecture.Name,
					Date = lecture.Date,
					TimeInMin = lecture.TimeInMin,
					CourseId = lecture.CourseId,
					CourseName = courseName
				});
		}
		/*
		 * Returns a List of lectures for specified user.
		 */
		//TODO: Should we take out the orderby in the return so we dont need to use ToList on it?
		public List<LectureDTO> GetLecturesForUser(string userId)
		{
			CourseService _CourseService = new CourseService(_db);
			
			IEnumerable<CourseDTO> coursesForUser = _CourseService.GetCoursesForUser(userId).ToList();
			if (coursesForUser == null)
				return null;

			List<LectureDTO> lectures = new List<LectureDTO>();
			string courseName = "";
			foreach (CourseDTO course in coursesForUser)
			{
				courseName = (course != null) ? course.Name : "";
				lectures.AddRange(
				 _db.Lectures.Where(lecture => lecture.CourseId == course.Id)
				.Select(lecture => new LectureDTO
				{
					Description = lecture.Description,
					Id = lecture.Id,
					Name = lecture.Name,
					Date = lecture.Date,
					TimeInMin = lecture.TimeInMin,
					CourseId = lecture.CourseId,
					CourseName = courseName
				}).ToList()
				);
			}

			return lectures.OrderBy(lecture => lecture.Date).ToList();
		}

		/*
		 * Get a lecture from database and makes it into ViewModel version of the lecture that have bit more info that it can store based on what is needed to show.
		 */
		public LectureDTO GetLecture(int lectureId)
		{
			if (IsValidLecture(lectureId) == false)
				return null;

			Course course = _db.Courses.Find(_db.Lectures.Find(lectureId).Id);
			string courseName = (course != null) ? course.Name : "";

			LectureDTO lectureDTO = new LectureDTO(_db.Lectures.SingleOrDefault(lecture => lecture.Id == lectureId));

			lectureDTO.CourseName = courseName;
			return lectureDTO;
		}
		/*
		 * Get a lecture from database
		 */
		internal Lecture GetLectureFromDb(int Id)
		{
			return _db.Lectures.Find(Id);
		}
		/*
		 * Updates the sent in lecture in the database by first selecting the lecture with corresponding id and then change the values based on the values sent in.
		 * then sends it to UpdateLecture that takes a lecture.
		 */
		//TODO: Throw exception if could not save
		public bool UpdateLecture(int id, string name, string description, DateTime date, int timeInMin, DateTime startTime)
		{
			Lecture lectureToUpdate = GetLectureFromDb(id);

			lectureToUpdate.Name = name;
			lectureToUpdate.Description = description;
			lectureToUpdate.TimeInMin = timeInMin;

			string indate = date.ToString("yyy/MM/dd");
			string inTime = startTime.ToString("t");
			lectureToUpdate.Date = StringToDate(indate, inTime);//date;

			return UpdateLecture(lectureToUpdate);

		}
		/*
		 * Updates the sent in lecture in the database
		 */
		//TODO: Throw exception if could not save
		public bool UpdateLecture(Lecture lectureToUpdate)
		{
			try
			{
				_db.Lectures.Attach(lectureToUpdate);
				_db.Entry(lectureToUpdate).State = EntityState.Modified;
				_db.SaveChanges();
			}
			catch (Exception ec)
			{
				Console.WriteLine(ec.Message);
				return false;
			}
			return true;
		}

		#region help functions
		/*
		 * Checks if the send in Id corresponds to a walid course
		 */
		//TODO Move out so you can use this in course and in this at the same time without having it 2 places
		public bool IsValidCourse(int courseId)
		{
			return _db.Courses.Find(courseId) != null;
		}
		/*
		 * Checks if the send in Id corresponds to a walid lecture
		 */
		public bool IsValidLecture(int lectureId)
		{
			return _db.Lectures.Find(lectureId) != null;
		}
		/*
		 *	date string must be in a format of yyyy-MM-dd otherwise it might not work.
		 *	time string must have only hour and minute.
		 *	
		 *	Takes two strings one as date and one as time and makes a Datetime of them.
		 */
		public DateTime StringToDate(string date, string time)
		{
			string indate = date +" "+ time;
			return DateTime.ParseExact(indate, "yyyy-MM-dd HH:mm",  CultureInfo.InvariantCulture);
		}
		#endregion
	}
}