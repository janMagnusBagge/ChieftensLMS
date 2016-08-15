using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChieftensLMS.Controllers.Api
{
	public class AssigmentApiController
	{
	}

	public JsonResult GetAssigments(int courseId)
	{
		var assigmentsForCourse = _AssignmentService.GetAssignmentForCourse(courseId);
		return assigmentsForCourse;
		return Json(assigmentsForCourse, JsonRequestBehavior.AllowGet);
	}
}