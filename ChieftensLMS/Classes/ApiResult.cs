using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChieftensLMS.Classes
{
	public static class ApiResult
	{
		public static JsonResult Success(object dataObject)
		{
			JsonResult returnObject = new JsonResult();
			returnObject.Data = new
			{
				data = dataObject,
				success = true
			};
			returnObject.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
			returnObject.ContentType = "application/json; charset=utf-8";

			return returnObject;
		}
		public static JsonResult Fail(string reason)
		{
			JsonResult returnObject = new JsonResult();
			returnObject.Data = new
			{
				reason = reason,
				success = false
			};
			returnObject.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
			returnObject.ContentType = "application/json; charset=utf-8";

			return returnObject;
		}
	}
}