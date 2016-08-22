using ChieftensLMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChieftensLMS.DAL
{

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		
		public IDbSet<TurnIn> TurnIns { get; set; }
		public IDbSet<SharedFile> SharedFiles { get; set; }
		public IDbSet<Course> Courses { get; set; }
		public IDbSet<Lecture> Lectures { get; set; }
		public IDbSet<Assignment> Assignments { get; set; }

		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false)
		{
		}

		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}



		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.HasDefaultSchema("dbo");

			modelBuilder.Entity<ApplicationUser>()
			.HasMany(s => s.Courses)
			.WithMany(c => c.Users)
			.Map(cs =>
			{
				cs.MapLeftKey("UserId");
				cs.MapRightKey("CourseId");
				cs.ToTable("CourseUsers");
			});
		}

	}

	//[Table(name: "CourseUsers")]
	//public class CourseUser
	//{
	//	[ForeignKey("CourseId")]
	//	public Course Course { get; set; }

	//	[ForeignKey("UserId")]
	//	public ApplicationUser User { get; set; }

	//	[Key, Column(Order = 0)]
	//	public string UserId { get; set; }

	//	[Key, Column(Order = 1)]
	//	public int CourseId { get; set; }
	//}
}