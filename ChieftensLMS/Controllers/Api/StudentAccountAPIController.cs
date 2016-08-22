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

		public StudentAccountAPIController()
		{
			_context = new ApplicationDbContext();
			_studentAccountService = new StudentAccountService(_context);
		}

		public ActionResult GetAll()
		{

			var users = _studentAccountService.GetAll();
			var usersWithRoles = users
				.Where(user => _studentAccountService.GetRolesUser(user.Id).Count() !=0)
				//.Where(user => UserManager.IsInRole(user.Id, _studentAccountService.GetAllRoles().FirstOrDefault(e => e.Id == user.Id)) == true)
				.Select(user =>	
				new
				{
					Email = user.UserName,
					Id = user.Id,
					Roles = _studentAccountService.GetRolesUser(user.Id)
				}
			);

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