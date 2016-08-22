using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ChieftensLMS.Services
{
	#region DTO Classes
	public class CourseDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
	}

	public class UserWithRolesDTO
	{
		public string Name { get; set; }
		public string SurName { get; set; }
		public IEnumerable<string> Roles { get; set; }
		public string UserId { get; set; }
	}

	public class UserDTO
	{
		public string Name { get; set; }
		public string SurName { get; set; }
		public string UserId { get; set; }
	}
	#endregion

	public class CourseService
	{
		ApplicationDbContext _db;

		public CourseService(ApplicationDbContext context)
		{
			_db = context;
		}

		// Klar
		public IEnumerable<CourseDTO> GetCoursesForUser(string userId)
		{
			if (isValidUser(userId) == false)
				return null;

			return _db.Courses.Where(course => course.Users.Any(user => user.Id == userId))
				.Select(course => new CourseDTO
				{
					Description = course.Description,
					Id = course.Id,
					Name = course.Name
				});
		}


		// KLAR
		public IEnumerable<CourseDTO> GetAllCourses()
		{
			return _db.Courses.Select(course => new CourseDTO()
			{
				Id = course.Id,
				Name = course.Name,
				Description = course.Description
			});
		}

		// KLAR
		public CourseDTO GetCourseById(int courseId, string asUser)
		{
			if (IsInCourse(courseId, asUser) == false)
				return null;

			Course course = _db.Courses.Find(courseId);

			return new CourseDTO()
			{
				Id = course.Id,
				Name = course.Name,
				Description = course.Description
			};
		}

		//KLAR
		public IEnumerable<UserWithRolesDTO> GetUsersWithRolesForCourse(int courseId, string asUser)
		{
			// for this to be true user and course have to exist so no need to check that
			if (IsInCourse(courseId, asUser) == false)
				return null;

			return _db.Users.Where(user => user.Courses.Any(course => course.Id == courseId))
				.Select(e => new UserWithRolesDTO()
				{
					Name = e.Name,
					SurName = e.SurName,
					UserId = e.Id,
					Roles = _db.Roles.Where(x => x.Users.Any(v => v.UserId == e.Id))
							.Select(r => r.Name).ToList()
				}).AsEnumerable();
		}

		//TODO: Should be deleted, after refactoring
		public bool HasUser(int courseId, string userId)
		{
			return _db.Courses.Any(course => course.Id == courseId && (course.Users.Any(user => user.Id == userId)));
		}

		//KLAR
		public bool RemoveUserFromCourse(int courseId, string userId, string asUser)
		{
			if ((IsValidCourse(courseId) && isValidUser(userId) && IsTeacherForCourse(asUser, courseId) && (userId != asUser)) == false)
				return false;

			ApplicationUser userWithCourse = _db.Users.Include(e => e.Courses).First(e => e.Id == userId && e.Courses.Any(c => c.Id == courseId));
			Course course = userWithCourse.Courses.FirstOrDefault(e => e.Id == courseId);
			userWithCourse.Courses.Remove(course);
			_db.Entry(userWithCourse).State = EntityState.Modified;
			_db.SaveChanges();

			return true;
		}

		//KLAR
		public IEnumerable<UserDTO> GetUsersNotInCourse(int courseId, string asUserId)
		{
			if ((IsTeacherForCourse(asUserId, courseId) && IsValidCourse(courseId)) == false)
				return null;

			return _db.Users.Where(user => user.Courses.All(course => course.Id != courseId))
					   .Select(user => new UserDTO()
					   {
						   Name = user.Name,
						   SurName = user.SurName,
						   UserId = user.Id
					   });
		}

		// Adds a user to a course, user must not be part of course and the user performing the operation must be a teacher + in course
		public bool AddUser(string userToAddId, int courseId, string asUserId)
		{
			if ((IsTeacherForCourse(asUserId, courseId) && IsInCourse(courseId, asUserId)) == false)
				return false;

			ApplicationUser userToAdd = _db.Users.Find(userToAddId);

			Course courseToAddTo = _db.Courses.FirstOrDefault(c => c.Id == courseId && (c.Users.Any(u => u.Id == userToAddId) == false));

			if (courseToAddTo == null)
				return false;

			courseToAddTo.Users.Add(userToAdd);
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

		// Checks if a user is in a role
		public bool IsTeacher(string userId)
		{
			return _db.Roles.Any(r => r.Name == "Teacher");
		}

		public int? CreateCourse(string name, string description, string asUser)
		{
			if (IsTeacher(asUser) == false)
				return null;

			Course courseToCreate = new Course() {
				Name = name,
				Description = description,
				Users = new List<ApplicationUser>() { _db.Users.Find(asUser) }
			};

			courseToCreate = _db.Courses.Add(courseToCreate);
			_db.SaveChanges();
			return courseToCreate.Id;
		}

		public bool EditCourse(int courseId, string name, string description, string asUser)
		{
			if (IsTeacherForCourse(asUser, courseId) == false)
				return false;

			Course courseToEdit = _db.Courses.Find(courseId);
			courseToEdit.Name = name;
			courseToEdit.Description = description;
			_db.SaveChanges();
			return true;
		}
	}
}