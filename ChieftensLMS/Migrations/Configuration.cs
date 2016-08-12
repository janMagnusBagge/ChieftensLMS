namespace ChieftensLMS.Migrations
{
	using DAL;
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.EntityFramework;
	using Models;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
			ContextKey = "ChieftensLMS.Models.ApplicationDbContext";
		}

		protected override void Seed(ApplicationDbContext context)
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
									Description ="Ni ska l�rar er riktigt hopp",
									Date = DateTime.Now,
									ExpirationDate = new DateTime(2020,10,20),
									TurnIns = new List<TurnIn>()
										{
											new TurnIn()
											{
												User = student, Date = DateTime.Now,
												FileName = "hopp_inl�mning",
												Name ="min hopp inl�mning",
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


			foundUser = userManager.FindByName("Teacher2@Teacher.com");
			if (foundUser != null) userManager.Delete(foundUser);
			foundUser = userManager.FindByName("Teacher3@Teacher.com");
			if (foundUser != null) userManager.Delete(foundUser);

			foundUser = userManager.FindByName("Student2@Student.com");
			if (foundUser != null) userManager.Delete(foundUser);
			foundUser = userManager.FindByName("Student3@Student.com");
			if (foundUser != null) userManager.Delete(foundUser);
			foundUser = userManager.FindByName("Student4@Student.com");
			if (foundUser != null) userManager.Delete(foundUser);

			ApplicationUser teacher2 = new ApplicationUser() { UserName = "Teacher2@Teacher.com", Email = "Teacher2@Teacher.com" };
			ApplicationUser teacher3 = new ApplicationUser() { UserName = "Teacher3@Teacher.com", Email = "Teacher3@Teacher.com" };

			userManager.Create(teacher2, "Password@123");
			userManager.Create(teacher3, "Password@123");

			userManager.AddToRole(teacher2.Id, "Teacher");
			userManager.AddToRole(teacher3.Id, "Teacher");

			ApplicationUser student2 = new ApplicationUser() { UserName = "Student2@Student.com", Email = "Student2@Student.com" };
			ApplicationUser student3 = new ApplicationUser() { UserName = "Student3@Student.com", Email = "Student3@Student.com" };
			ApplicationUser student4 = new ApplicationUser() { UserName = "Student4@Student.com", Email = "Student4@Student.com" };

			userManager.Create(student2, "Password@123");
			userManager.Create(student3, "Password@123");
			userManager.Create(student4, "Password@123");

			userManager.AddToRole(student2.Id, "Student");
			userManager.AddToRole(student3.Id, "Student");
			userManager.AddToRole(student4.Id, "Student");


			context.Courses.AddOrUpdate(c => c.Name,
					new Course()
					{
						Name = "Skrivar kurs i Rosa cykel i valfri f�rg",
						Assignments = new List<Assignment>()
							{
								new Assignment() 
								{
									Name = "Rosa cykel",
									Description ="Skriva rosa cykel 50 g�nger",
									Date = DateTime.Now,
									ExpirationDate = new DateTime(2020,10,20),
									TurnIns = new List<TurnIn>()
										{
											new TurnIn()
											{
												User = student, Date = DateTime.Now,
												FileName = "Rosa_inl�mning",
												Name ="min Rosa cykel inl�mning",
											}
										}
								},
								new Assignment() 
								{
									Name = "Rosa cykel i valfri f�rg",
									Description ="Skriva rosa cykel i valfri f�rg 50 g�nger",
									Date = DateTime.Now,
									ExpirationDate = new DateTime(2020,10,20),
									TurnIns = new List<TurnIn>()
										{
											new TurnIn()
											{
												User = student, Date = DateTime.Now,
												FileName = "Valfri_f�rg_inl�mning",
												Name ="min Rosa cykel i valfri f�rg inl�mning",
											}
										}
								}
							},
						Lectures = new List<Lecture>()
							{
								new Lecture()
								{
									Date = DateTime.Now,
									Name = "Lektion i stavning",
									Description = "l�r sig stava"
								},
								new Lecture()
								{
									Date = DateTime.Now,
									Name = "Lektion i skriva meningar",
									Description = "Kunna skriva meningar"
								}
							},
						SharedFiles = new List<SharedFile>()
						{
							new SharedFile()
							{
								Date = DateTime.Now,
								Name = "Meningsuppbygnad",
								FileName = "Meningsuppbygnad.pdf",
								User = teacher2
							}
						},
						Users = new List<ApplicationUser>() { teacher2, student,student2,student3 }
					}
				);

			context.Courses.AddOrUpdate(c => c.Name,
					new Course()
					{
						Name = "G� p� h�nder",
						Assignments = new List<Assignment>()
							{
								new Assignment() 
								{
									Name = "St� p� h�nder mot v�gg",
									Description ="L�ra sig st� p� h�nder",
									Date = DateTime.Now,
									ExpirationDate = new DateTime(2020,10,20),
									TurnIns = new List<TurnIn>()
										{
											new TurnIn()
											{
												User = student, Date = DateTime.Now,
												FileName = "Sta_pa_hander_mot_vagg",
												Name ="Min st� p� h�nder mot v�gg inl�mning",
											}
										}
								},
								new Assignment() 
								{
									Name = "St� p� h�nder utan v�gg",
									Description ="L�ra sig st� p� h�nder",
									Date = DateTime.Now,
									ExpirationDate = new DateTime(2020,10,20),
									TurnIns = new List<TurnIn>()
										{
											new TurnIn()
											{
												User = student, Date = DateTime.Now,
												FileName = "Sta_pa_hander_inl�mning",
												Name ="Min st� p� h�nder utan v�gg inl�mning",
											}
										}
								},
								new Assignment() 
								{
									Name = "G� p� h�nder",
									Description ="L�ra sig g� p� h�nder",
									Date = DateTime.Now,
									ExpirationDate = new DateTime(2020,10,20),
									TurnIns = new List<TurnIn>()
										{
											new TurnIn()
											{
												User = student, Date = DateTime.Now,
												FileName = "Ga_Pa_Hander_inlamning",
												Name ="Min g� p� h�nder inl�mning",
											}
										}
								}
							},
						Lectures = new List<Lecture>()
							{
								new Lecture()
								{
									Date = DateTime.Now,
									Name = "Lektion i arm �vningar",
									Description = "Lite olika arm �vning f�r att kunna st� p� h�nder"
								},
								new Lecture()
								{
									Date = DateTime.Now,
									Name = "Lektion i st� p� h�nder och olika tekniker",
									Description = "St� p� h�nder"
								}
							},
						SharedFiles = new List<SharedFile>()
						{
							new SharedFile()
							{
								Date = DateTime.Now,
								Name = "�vningar",
								FileName = "�vningar.pdf",
								User = teacher2
							}
						},
						Users = new List<ApplicationUser>() { teacher3, student4, student2, student3 }
					}
				);
		}
	}
}
