﻿using ChieftensLMS.DAL;
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
		public TimeSpan StartTime { get; set; }

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
			StartTime = new TimeSpan(Date.Hour,Date.Minute,Date.Second);
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

		//TODO Move out so you can use this in course and in this at the same time without having it 2 places
		public bool IsValidCourse(int courseId)
		{
			return _db.Courses.Find(courseId) != null;
		}

		public bool IsValidLecture(int lectureId)
		{
			return _db.Lectures.Find(lectureId) != null;
		}
	}
}