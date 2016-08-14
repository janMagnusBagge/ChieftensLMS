using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChieftensLMS.DAL
{
	public class UnitOfWork : IDisposable
	{
		private bool _disposed = false;
		private LMSDbContext _context;
		private GenericRepository<UserProfile> _userProfileRepository;
		private GenericRepository<Assignment> _assignmentRepository;
		private GenericRepository<Course> _courseRepository;
		private GenericRepository<Lecture> _lectureRepository;
		private GenericRepository<SharedFile> _sharedFileRepository;
		private GenericRepository<TurnIn> _turnInRepository;

		public UnitOfWork(LMSDbContext context)
		{
			_context = context;
			_userProfileRepository = new GenericRepository<UserProfile>(_context);
			_assignmentRepository = new GenericRepository<Assignment>(_context);
			_courseRepository = new GenericRepository<Course>(_context);
			_lectureRepository = new GenericRepository<Lecture>(_context);
			_sharedFileRepository = new GenericRepository<SharedFile>(_context);
			_turnInRepository = new GenericRepository<TurnIn>(_context);
		}

		public GenericRepository<UserProfile> ApplicationUser
		{
			get
			{
				return _userProfileRepository;
			}
		}

		public GenericRepository<Assignment> AssignmentRepository
		{
			get
			{
				return _assignmentRepository;
			}
		}

		public GenericRepository<Course> CourseRepository
		{
			get
			{
				return _courseRepository;
			}
		}

		public GenericRepository<Lecture> LectureRepository
		{
			get
			{
				return _lectureRepository;
			}
		}

		public GenericRepository<SharedFile> SharedFileRepository
		{
			get
			{
				return _sharedFileRepository;
			}
		}

		public GenericRepository<TurnIn> TurnInRepository
		{
			get
			{
				return _turnInRepository;
			}
		}

		public void Save()
		{
			_context.SaveChanges();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					_context.Dispose();
				}
			}
			this._disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}

}