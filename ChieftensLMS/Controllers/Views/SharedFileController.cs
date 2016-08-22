using ChieftensLMS.Classes;
using System.Web;
using System.Web.Mvc;

namespace ChieftensLMS.Controllers
{
    public class SharedFileController : Controller
    {
		public ActionResult ForCourse(int? id)
        {
			return View(id);
        }

		public ActionResult Mine()
		{
			return View();
		}

		public ActionResult Upload(int? id)
		{
			return View(id);
		}
	}
}
