//using ChieftensLMS.DAL;
//using ChieftensLMS.Models;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.AspNet.Identity.Owin;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
using ChieftensLMS.Classes;
using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using ChieftensLMS.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace ChieftensLMS.Services
{
	public class StudentAccountService
	{
		ApplicationDbContext _db;

		/*
		 * Initiate the Account service
		 */
		public StudentAccountService(ApplicationDbContext context)
		{
			_db = context;
		}

		/*
		 * Returns a list of all accounts
		 */
		public IEnumerable<ApplicationUser> GetAll()
		{
			//RoleManager<IdentityRole> _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
			return _db.Users.ToList();
		}
		/*
		 * returns specific user / account. 
		 * Id is a string cause the account id is a string.
		 */
		public ApplicationUser GetUser(string id)
		{
			//RoleManager<IdentityRole> _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
			return _db.Users.FirstOrDefault(u => u.Id == id);
		}

		/*
		 * Return All roles
		 */
		public IEnumerable<IdentityRole> GetAllRoles()
		{
			
			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
			var role = roleManager.Roles.ToList();
			return role;
		}
		/*
		 * Return roles for a specifik user / account
		 */
		public IEnumerable<string> GetRolesUser(string Id)
		{
			var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
			return userManager.GetRoles(Id);
		}

	}
}