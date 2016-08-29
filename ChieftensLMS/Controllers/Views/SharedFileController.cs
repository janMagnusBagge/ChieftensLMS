using ChieftensLMS.Classes;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Web.Mvc;

namespace ChieftensLMS.Controllers
{
    public class SharedFileController : LMSController
    {
		public ActionResult Mine()
		{
			return View();
		}

		public ActionResult Course(int? id)
		{
			return View(id);
		}
	}
}
