using ChieftensLMS.Classes;
using ChieftensLMS.DAL;
using ChieftensLMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChieftensLMS.Controllers.Api
{
    public class DebugApiController : Controller
    {


		public ActionResult AllUsers()
		{
			var _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			var users = _userManager.Users.ToList();

			var usersAndRoles = users.Select(user =>
				new
				{
					Email = user.UserName,
					Id = user.Id,
					Password = "Password@123",
					Roles = _userManager.GetRoles(user.Id)

				}
			);

			var currentUser = new
			{
				Email = User.Identity.GetUserName(),
				Id = User.Identity.GetUserId(),
				Roles = _userManager.GetRoles(User.Identity.GetUserId())
			};

			var returnData = new
			{
				CurrentUser = currentUser,
				AllUsers = usersAndRoles
			};

			return ApiResult.Success(returnData);

		}

		public async Task<ActionResult> LoginAs(string email)
		{
			var signinManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();

			await signinManager.PasswordSignInAsync(email, "Password@123", true, false);

			return ApiResult.Success(new { });
		}


        // GET: DebugApi
        public async Task<ActionResult> Index()
        {
			var signinManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
			await signinManager.PasswordSignInAsync("T13@lms.com", "Password@123", true,false);
			var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			 
			
			return Content("da");

        }
    }
}