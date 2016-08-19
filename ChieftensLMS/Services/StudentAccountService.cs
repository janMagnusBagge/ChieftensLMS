using ChieftensLMS.DAL;
using ChieftensLMS.Models;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChieftensLMS.Services
{
	public class StudentAccountService
	{
		ApplicationDbContext _db;


		public StudentAccountService(ApplicationDbContext context)
		{
			_db = context;
		}

		public IEnumerable<ApplicationUser> GetAll()
		{
			//RoleManager<IdentityRole> _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
			return _db.Users.ToList();
		}

	}
}