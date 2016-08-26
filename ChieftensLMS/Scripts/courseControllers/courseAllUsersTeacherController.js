(function () {

	angular.module("app").controller("courseAllUsersTeacherController", function ($scope, ApiService) {

		$scope.apiData = {};
		var userIdToRemove = -1;

		var onSuccess = function (data) {
			$scope.apiData = data;
		};

		var onFail = function (response) {
			alert(response.Reason);
		};

		var onRemoveSuccess = function (data) {
			var index = -1;
			for (var i = 0; i < $scope.apiData.Users.length; i++) {
				if ($scope.apiData.Users[i].UserId == data.UserId) {
					index = i;
					break;
				}
			}

			if (index != -1)
				$scope.apiData.Users.splice(index, 1);

		};

		var onRemoveFail = function (response) {
			alert(response.Reason);
		};


		$scope.RemoveUserFromCourse = function (id, courseId) {
			ApiService.Get("/CourseApi/RemoveUserFromCourse", onRemoveSuccess, onRemoveFail, { userId: id, courseId: courseId });
		}

		$scope.GetAllUsers = function(id)
		{
			ApiService.Get("/CourseApi/AllUsers", onSuccess, onFail, { id: id });
		}
			
	});

}());