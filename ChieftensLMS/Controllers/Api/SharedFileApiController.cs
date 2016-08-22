using System.Web;
using System.Web.Mvc;
using ChieftensLMS.Services;
using Microsoft.AspNet.Identity;
using ChieftensLMS.Classes;

namespace ChieftensLMS.Controllers
{
	public class SharedFileApiController : LMSController
	{
		private SharedFileService _sharedFileService = LMSHelper.GetSharedFileService();
		private CourseService _courseService = LMSHelper.GetCourseService();

		[Authorize]
		public ActionResult Mine()
		{
			var result = _sharedFileService.GetSharedFilesWithCourseForUser(_currentUserId);

			if (result == null)
				return ApiResult.Fail("");
			else
				return ApiResult.Success(new { SharedFiles = result });
		}

		//FIX EVERYTHING BElOW THIS, ALSO CHECK COMMENTS ON THIS AND THE OTHER SERVICE
		[Authorize]
		public ActionResult ForCourse(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid request");

			var result = _sharedFileService.GetSharedFilesWithOwnerForCourse((int)id, _currentUserId);

			if (result == null)
				return ApiResult.Fail("");
			else
				return ApiResult.Success(new { UserId = _currentUserId, SharedFiles = result });
		}

		[Authorize]
		public ActionResult Download(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid request");

			var result = _sharedFileService.GetById((int)id, _currentUserId);

			if (result == null)
				return ApiResult.Fail("");

			string physicalFileToReturn = _sharedFileService.GetPhysicalPath(result.Id);
				if (physicalFileToReturn == null)
					return ApiResult.Fail("");

			string mimeType = MimeMapping.GetMimeMapping(result.FileName);

			return File(physicalFileToReturn, mimeType, result.FileName);
		}

		[Authorize]
		public ActionResult Delete(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Bad request");

			var result = _sharedFileService.Delete((int)id, _currentUserId);

			if (result == false)
				return ApiResult.Fail("");
			else
				return ApiResult.Success(new { Id = id});
		}

		[Authorize]
		public ActionResult Upload(HttpPostedFileBase file, string name, int? courseId)
		{
			if (string.IsNullOrWhiteSpace(file.FileName) || string.IsNullOrWhiteSpace(name) || courseId == null)
				return ApiResult.Fail("");

			int? result = _sharedFileService.AddSharedFile((int)courseId, _currentUserId, name, file.FileName, file.InputStream);
			if (result == null)
				return ApiResult.Fail("");
			else
				return ApiResult.Success(new { FileId = result });
		}

	}
}
