using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChieftensLMS.Services
{
	public class LectureDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime Date { get; set; }
		public string Description { get; set; }
		public int CourseId { get; set; }
		public int TimeInMin { get; set; }
	}
	public class LecturesService
	{
		ApplicationDbContext _db;

		public LecturesService(ApplicationDbContext context)
		{
			_db = context;
		}

		public IEnumerable<LectureDTO> GetCoursesForCourse(int courseId)
		{

			return _db.Lectures.Where(lecture => lecture.CourseId == courseId)
				.Select(lecture => new LectureDTO
				{
					Description = lecture.Description,
					Id = lecture.Id,
					Name = lecture.Name,
					Date = lecture.Date,
					TimeInMin = lecture.TimeInMin
				});
		}
	}
}