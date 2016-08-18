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

	public class UserSeeder
	{
		private LMSDbContext _context;
		private UserManager<ApplicationUser> _userManager;
		private RoleManager<IdentityRole> _roleManager;

		public UserSeeder(LMSDbContext context)
		{
			var _userContext = new ApplicationDbContext();
			_userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_userContext));
			_roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_userContext));
			_context = context;
		}

		public UserProfile CreateUserWithRole(string userName, string password, string role, string name, string surname)
		{
			var foundUser = _userManager.FindByName(userName);
			if (foundUser != null)
				return _context.UserProfile.Find(foundUser.Id);

			ApplicationUser user = new ApplicationUser() { UserName = userName, Email = userName };
			_userManager.Create(user, password);

			var foundRole = _roleManager.FindByName(role);
			if (foundRole == null)
				_roleManager.Create(new IdentityRole() { Name = role });

			_userManager.AddToRole(user.Id, role);

			UserProfile returnProfile = new UserProfile() { Id = user.Id, Name = name, SurName = surname };

			_context.UserProfile.Add(returnProfile);
			_context.SaveChanges();
			return returnProfile;
		}

	}


	internal sealed class Configuration : DbMigrationsConfiguration<LMSDbContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(LMSDbContext context)
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

			UserSeeder seeder = new UserSeeder(context);

			List<UserProfile> userProfiles = new List<UserProfile>();
			for (int i = 0; i < 20; i++)
				userProfiles.Add(seeder.CreateUserWithRole("T" + i + "@lms.com", "Password@123", "Teacher", "L�rare_" + i, "Efternamn" + i));
			for (int i = 0; i < 20; i++)
				userProfiles.Add(seeder.CreateUserWithRole("S" + i + "@lms.com", "Password@123", "Student", "Student_" + i, "Efternamn" + i));


			for (int i = 0; i < 20; i++)
			{
				Course course = new Course()
				{
					Description = "Kursbeskrivning " + i,
					Name = "Kursnamn " + i,
					Assignments = new List<Assignment>(),
					Lectures = new List<Lecture>(),
					SharedFiles = new List<SharedFile>(),
					Users = new List<UserProfile>()
				};

				for (int a = 0; a < 20; a++)
				{
					course.Lectures.Add(new Lecture()
					{
						Date = DateTime.Now,
						Name = course.Name + " - lektion " + a,
						Description = "Beskrivning f�r " + course.Name + " - lektion " + a
					});
				}

				for (int x = 0; x < 40; x++)
				{
					course.SharedFiles.Add(
					new SharedFile()
					{
						Date = DateTime.Now,
						Name = course.Name + "_" + userProfiles[x].Name + "_uppladdning",
						FileName = course.Name + "_" + userProfiles[x].Name + "_uppladdning" + ".txt",
						User = userProfiles[x]
					});
				}

				for (int x = 0; x < 40; x++)
				{
					course.Users.Add(userProfiles[x]);
				}

				for (int x = 0; x < 5; x++)
				{
					Assignment assignment = new Assignment()
					{
						Name = course.Name + "_uppgift_" + x,
						Description = "Beskrivning_" + course.Name + "_uppgift_" + x,
						Date = DateTime.Now,
						ExpirationDate = new DateTime(2020, 10, 20)
					};

					assignment.TurnIns = new List<TurnIn>();
					course.Assignments.Add(assignment);

					for (int v = 0; v < 10; v++)
					{
						TurnIn turnIn = new TurnIn()
						{
							User = userProfiles[v],
							Date = DateTime.Now,
							FileName = course.Name + "_inl�mning_" + x + ".txt",
							Name = course.Name + "_inl�mning_" + x + ".txt"
						};

						assignment.TurnIns.Add(turnIn);
					}
				}


				context.Courses.AddOrUpdate(c => c.Name, course);
			}





		}
	}
}
