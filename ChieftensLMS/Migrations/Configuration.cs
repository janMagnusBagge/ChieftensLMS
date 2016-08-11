namespace ChieftensLMS.Migrations
{
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.EntityFramework;
	using Models;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<ChieftensLMS.Models.ApplicationDbContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
			ContextKey = "ChieftensLMS.Models.ApplicationDbContext";
		}

		protected override void Seed(ChieftensLMS.Models.ApplicationDbContext context)
		{
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data. E.g.
			//
			//    context.People.AddOrUpdate(
			//      p => p.FullName,
			//      new Person { FullName = "Andrew Peters" },
			//      new Person { FullName = "Brice Lambson" },
			//      new Person { FullName = "Rowan Miller" }
			//    );
			//



			var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

			ApplicationUser foundUser = null;

			foundUser = userManager.FindByName("Teacher@Teacher.com");
			if (foundUser != null) userManager.Delete(foundUser);

			foundUser = userManager.FindByName("Student@Student.com");
			if (foundUser != null) userManager.Delete(foundUser);

			ApplicationUser teacher = new ApplicationUser() { UserName = "Teacher@Teacher.com", Email = "Teacher@Teacher.com" };
			ApplicationUser student = new ApplicationUser() { UserName = "Student@Student.com", Email = "Student@Student.com" };
			userManager.Create(teacher, "Password@123");
			userManager.Create(student, "Password@123");

			IdentityRole foundRole = null;

			foundRole = roleManager.FindByName("Teacher");
			if (foundRole != null) roleManager.Delete(foundRole);

			foundRole = roleManager.FindByName("Student");
			if (foundRole != null) roleManager.Delete(foundRole);


			roleManager.Create(new IdentityRole() { Name = "Teacher" });
			roleManager.Create(new IdentityRole() { Name = "Student" });

			userManager.AddToRole(teacher.Id, "Teacher");
			userManager.AddToRole(student.Id, "Student");



			context.Courses.AddOrUpdate(c => c.Name,
					new Course()
					{
						Name = "Rosa gymnastik",
						Assignments = new List<Assignment>()
							{
								new Assignment() {
									Name = "Hoppa hopprep",
									Description ="Ni ska lärar er riktigt hopp",
									Date = DateTime.Now,
									ExpirationDate = new DateTime(2020,10,20),
									TurnIns = new List<TurnIn>()
										{
											new TurnIn()
											{
												User = student, Date = DateTime.Now,
												FileName = "hopp_inlämning",
												Name ="min hopp inlämning",
											}
										}
							}
							},
						Lectures = new List<Lecture>()
							{
								new Lecture()
								{
									Date = DateTime.Now,
									Name = "Lektion i databaser",
									Description = "Ge upp"
								}
							},
						SharedFiles = new List<SharedFile>()
						{
							new SharedFile()
							{
								Date = DateTime.Now,
								Name = "Chieften's plans",
								FileName = "chieftensPlans.pdf",
								User = teacher
							}
						},
						Users = new List<ApplicationUser>() { teacher, student }
					}
				);
		}
	}
}
