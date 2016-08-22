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
	}
}
