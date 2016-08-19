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
			var usersAndRoles = users.Select(user =>	
				new
				{
					Email = user.UserName,
					Id = user.Id,
					Password = "Password@123",
					Roles = _userManager.GetRoles(user.Id)

				}
			);

			return ApiResult.Success(new { Blaha = Blaha });
		}
    }
}