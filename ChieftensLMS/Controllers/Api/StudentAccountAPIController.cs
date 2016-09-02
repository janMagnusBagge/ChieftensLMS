using ChieftensLMS.Classes;
using ChieftensLMS.DAL;
using ChieftensLMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChieftensLMS.Controllers.Api
{
    public class StudentAccountAPIController : Controller
    {
        private ApplicationDbContext _context;
		private StudentAccountService _studentAccountService;

		/*
		 * Initiate the Account api
		 */
		public StudentAccountAPIController()
		{
			_context = new ApplicationDbContext();
			_studentAccountService = new StudentAccountService(_context);
		}
		/*
		 * Returns a one list with all accounts and their assosiated roles and another list with accounts that do not have roles.
		 */
		public ActionResult GetAll()
		{

			var users = _studentAccountService.GetAll();
			//variable for list of users that have roles
			var usersWithRoles = users
				.Where(user => _studentAccountService.GetRolesUser(user.Id).Count() !=0)
				.Select(user =>	
				new
				{
					Email = user.UserName,
					Id = user.Id,
					Roles = _studentAccountService.GetRolesUser(user.Id) //gets the roles for specifik user
				}
			);

			//variable for list of users that do not have roles
			var usersWithoutRoles = users
				.Where(user => _studentAccountService.GetRolesUser(user.Id).Count() == 0)
				.Select(user =>
				new
				{
					Email = user.UserName,
					Id = user.Id,
				}
			);

			var returnData = new
			{
				usersWithoutRoles = usersWithoutRoles,
				usersWithRoles = usersWithRoles
			};

			return ApiResult.Success(new { returnData = returnData });
		}
    }
}