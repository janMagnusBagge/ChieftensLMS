using ChieftensLMS.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChieftensLMS.Classes
{
	public class LMSController : Controller
	{
		protected ApplicationDbContext _context = LMSHelper.GetApplicationContext();
		protected ApplicationUserManager _userManager = LMSHelper.GetUserManager();
		protected RoleManager<IdentityRole> _roleManger = LMSHelper.GetRoleManager();
		protected string _currentUserId = LMSHelper.GetCurrectUserId();
	}
}