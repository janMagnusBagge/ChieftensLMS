
(function () {

	angular.module("app").controller("courseAllUsersController", function ($scope, ApiService) {

		$scope.apiData = {};

		var onSuccess = function (data) {
			$scope.apiData.users = data.Users;
		};

		var onFail = function (response) {
			alert(response.Reason);
		};

		$scope.GetAllUsers = function (id) {
			ApiService.Get("/CourseApi/AllUsers", onSuccess, onFail, { id: id });
		}

	});

}());