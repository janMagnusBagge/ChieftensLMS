﻿using ChieftensLMS.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChieftensLMS.DAL
{
	public class LMSDbContext : DbContext
	{
		public IDbSet<TurnIn> TurnIns { get; set; }
		public IDbSet<SharedFile> SharedFiles { get; set; }
		public IDbSet<Course> Courses { get; set; }
		public IDbSet<Lecture> Lectures { get; set; }
		public IDbSet<Assignment> Assignments { get; set; }
		public IDbSet<UserProfile> UserProfile { get; set; }


		public LMSDbContext()
			: base("DefaultConnection")
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.HasDefaultSchema("dbo");

			modelBuilder.Entity<UserProfile>()
				.HasMany(p => p.Courses)
				.WithMany(s => s.Users)
				.Map(c =>
				{
					c.MapLeftKey("UserId");
					c.MapRightKey("CourseId");
					c.ToTable("CourseUsers");
				});
		}

	}
}