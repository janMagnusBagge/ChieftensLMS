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

		public UserProfile CreateUserWithRole(string userName, string password, string role)
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

			UserProfile returnProfile = new UserProfile() { Id = user.Id, Name = "Namn", SurName = "Efternamn" };

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

			var teacher = seeder.CreateUserWithRole("Teacher@Teacher.com", "Password@123", "Teacher");
			var student = seeder.CreateUserWithRole("Student@Student.com", "Password@123", "Student");

			context.Courses.AddOrUpdate(c => c.Name,
					new Course()
					{
						Description = "Fifty shades of rosa",
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
						Users = new List<UserProfile>() { teacher, student }
					}
				);



			var teacher2 = seeder.CreateUserWithRole("Teacher2@Teacher.com", "Password@123", "Teacher");
			var teacher3 = seeder.CreateUserWithRole("Teacher3@Teacher.com", "Password@123", "Teacher");

			var student2 = seeder.CreateUserWithRole("Student2@Student.com", "Password@123", "Teacher");
			var student3 = seeder.CreateUserWithRole("Student3@Student.com", "Password@123", "Teacher");
			var student4 = seeder.CreateUserWithRole("Student4@Student.com", "Password@123", "Teacher");

			context.Courses.AddOrUpdate(c => c.Name,
					new Course()
					{
						Description = "Förklaring om hur man skriver om cyklar",
						Name = "Skrivar kurs i Rosa cykel i valfri färg",
						Assignments = new List<Assignment>()
							{
									new Assignment()
									{
										Name = "Rosa cykel",
										Description ="Skriva rosa cykel 50 gånger",
										Date = DateTime.Now,
										ExpirationDate = new DateTime(2020,10,20),
										TurnIns = new List<TurnIn>()
											{
												new TurnIn()
												{
													User = student, Date = DateTime.Now,
													FileName = "Rosa_inlämning",
													Name ="min Rosa cykel inlämning",
												}
											}
									},
									new Assignment()
									{
										Name = "Rosa cykel i valfri färg",
										Description ="Skriva rosa cykel i valfri färg 50 gånger",
										Date = DateTime.Now,
										ExpirationDate = new DateTime(2020,10,20),
										TurnIns = new List<TurnIn>()
											{
												new TurnIn()
												{
													User = student, Date = DateTime.Now,
													FileName = "Valfri_färg_inlämning",
													Name ="min Rosa cykel i valfri färg inlämning",
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
										Description = "lär sig stava"
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
						Users = new List<UserProfile>() { teacher2, student, student2, student3 }
					}
				);

			context.Courses.AddOrUpdate(c => c.Name,
					new Course()
					{
						Description = "Bättre än att gå på fötter",
						Name = "Gå på händer",
						Assignments = new List<Assignment>()
							{
									new Assignment()
									{
										Name = "Stå på händer mot vägg",
										Description ="Lära sig stå på händer",
										Date = DateTime.Now,
										ExpirationDate = new DateTime(2020,10,20),
										TurnIns = new List<TurnIn>()
											{
												new TurnIn()
												{
													User = student, Date = DateTime.Now,
													FileName = "Sta_pa_hander_mot_vagg",
													Name ="Min stå på händer mot vägg inlämning",
												}
											}
									},
									new Assignment()
									{
										Name = "Stå på händer utan vägg",
										Description ="Lära sig stå på händer",
										Date = DateTime.Now,
										ExpirationDate = new DateTime(2020,10,20),
										TurnIns = new List<TurnIn>()
											{
												new TurnIn()
												{
													User = student, Date = DateTime.Now,
													FileName = "Sta_pa_hander_inlämning",
													Name ="Min stå på händer utan vägg inlämning",
												}
											}
									},
									new Assignment()
									{
										Name = "Gå på händer",
										Description ="Lära sig gå på händer",
										Date = DateTime.Now,
										ExpirationDate = new DateTime(2020,10,20),
										TurnIns = new List<TurnIn>()
											{
												new TurnIn()
												{
													User = student, Date = DateTime.Now,
													FileName = "Ga_Pa_Hander_inlamning",
													Name ="Min gå på händer inlämning",
												}
											}
									}
							},
						Lectures = new List<Lecture>()
							{
									new Lecture()
									{
										Date = DateTime.Now,
										Name = "Lektion i arm övningar",
										Description = "Lite olika arm övning för att kunna stå på händer"
									},
									new Lecture()
									{
										Date = DateTime.Now,
										Name = "Lektion i stå på händer och olika tekniker",
										Description = "Stå på händer"
									}
							},
						SharedFiles = new List<SharedFile>()
						{
								new SharedFile()
								{
									Date = DateTime.Now,
									Name = "Övningar",
									FileName = "Övningar.pdf",
									User = teacher2
								}
						},
						Users = new List<UserProfile>() { teacher3, student4, student2, student3 }
					}
				);
		}
	}
}
