(function () {

	angular.module("app").controller("courseAddUserTeacherController", function ($scope, ApiService) {

		$scope.apiData = {};
		var userIdToRemove = -1;

		var onSuccess = function (data) {
			$scope.apiData = data;
		};

		var onFail = function (response) {
			alert(response.Reason);
		};

		var onAddSuccess = function (data) {
			var index = -1;
			for (var i = 0; i < $scope.apiData.Users.length; i++) {
				if ($scope.apiData.Users[i].UserId == data.UserId) {
					index = i;
					break;
				}
			}

			if (index != -1)	
				$scope.apiData.Users.splice(index, 1);

			$scope.$emit("courseAddUserTeacherController_added");
		};

		var onAddFail = function (response) {
			alert(response.Reason);
		};

		$scope.AddUserToCourse = function (id, courseId) {

			userIdToRemove = id;
			ApiService.Get("/CourseApi/AddUser", onAddSuccess, onAddFail, { userId: id, courseId: courseId });
		}

		$scope.GetUsersNotInCourse = function ()
		{
			ApiService.Get("/CourseApi/UsersNotInCourse", onSuccess, onFail, { id: $scope.id });
		}

		$scope.$on("courseAddUserTeacherController_refresh", function () {
			$scope.GetUsersNotInCourse();
		})

	});

}());