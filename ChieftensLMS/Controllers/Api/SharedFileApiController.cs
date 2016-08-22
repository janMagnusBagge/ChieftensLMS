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

			CourseDTO course = _courseService.GetCourseById((int)id, _currentUserId);

			if (course == null)
				return ApiResult.Fail("Course does not exist");

			// Needs to be either a student that takes the course or any teacher
			if (_userManager.IsInRole(_currentUserId, "Teacher") || _courseService.HasUser(course.Id, _currentUserId))
			{
				return ApiResult.Success(new { UserId = _currentUserId,  SharedFiles = _sharedFileService.GetSharedFilesWithOwnerForCourse(course.Id) });
			}
			else
			{
				return ApiResult.Fail("No access");
			}
		}

		[Authorize]
		public ActionResult Download(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Invalid request");

			SharedFileDTO sharedFile = _sharedFileService.GetById((int)id);

			if (sharedFile == null)
				return ApiResult.Fail("Invalid file");
			
			if (_userManager.IsInRole(_currentUserId, "Teacher") || _courseService.HasUser(sharedFile.CourseId , _currentUserId) || sharedFile.OwnerId == _currentUserId)
			{
				string physicalFileToReturn = _sharedFileService.GetPhysicalPath(sharedFile.Id);

				if (physicalFileToReturn == null)
					return ApiResult.Fail("File deleted from server");

				string mimeType = MimeMapping.GetMimeMapping(sharedFile.FileName);

				return File(physicalFileToReturn, mimeType, sharedFile.FileName);
			}
			else
				return ApiResult.Fail("No Access");
		}

		[Authorize]
		public ActionResult Delete(int? id)
		{
			if (id == null)
				return ApiResult.Fail("Bad request");

			var sharedFile = _sharedFileService.GetById((int)id);

			if (sharedFile == null)
				return ApiResult.Fail("Invalid file");

			// Only delete if the file is owned by user or ANY teacher
			if (_userManager.IsInRole(_currentUserId, "Teacher") || sharedFile.OwnerId == _currentUserId)
			{
				_sharedFileService.Delete(sharedFile.Id);
				return ApiResult.Success(new { Id = id });
			}
			else
				return ApiResult.Fail("No access to delete this shared file");
			

		}

	}
}
