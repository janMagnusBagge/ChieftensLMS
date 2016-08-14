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
						Description = "F�rklaring om hur man skriver om cyklar",
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
						Users = new List<UserProfile>() { teacher2, student, student2, student3 }
					}
				);

			context.Courses.AddOrUpdate(c => c.Name,
					new Course()
					{
						Description = "B�ttre �n att g� p� f�tter",
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
						Users = new List<UserProfile>() { teacher3, student4, student2, student3 }
					}
				);
		}
	}
}
