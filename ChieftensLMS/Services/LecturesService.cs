using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using System;
using System.Collections.Generic;
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
	}
	#endregion

	public class LecturesService
	{
		ApplicationDbContext _db;

		public LecturesService(ApplicationDbContext context)
		{
			_db = context;
		}

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
					CourseName = courseName
				});
		}

		//TODO: Should we take out the orderby in the return so we dont need to use ToList on it?
		public List<LectureDTO> GetLecturesForUser(string userId)
		{
			CourseService _CourseService = new CourseService(_db);
			
			//IEnumerable<CourseDTO> coursesForUser = _CourseService.GetCoursesForUser(userId);

			List<LectureDTO> lectures = new List<LectureDTO>();
			string courseName = "";
			foreach(CourseDTO course in _CourseService.GetCoursesForUser(userId))
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
					CourseName = courseName
				})
				);
			}

			return lectures.OrderBy(lecture => lecture.Date).ToList();
		}

		//TODO Move out so you can use this in course and in this at the same time without having it 2 places
		public bool IsValidCourse(int courseId)
		{
			return _db.Courses.Find(courseId) != null;
		}
	}
}