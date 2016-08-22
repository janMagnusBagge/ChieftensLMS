using ChieftensLMS.DAL;
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
		protected string _currentUserId = LMSHelper.GetCurrectUserId();
	}
}