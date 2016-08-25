(function () {

	angular.module("app").controller("courseAllController", function ($scope, ApiService) {

		$scope.apiData = {};

		var onSuccess = function (data) {
			$scope.apiData.courses = data.Courses;
		};

		var onFail = function (response) {
			alert(response.Reason);
		};

		ApiService.Get("/CourseApi/All", onSuccess, onFail);

	});

}());