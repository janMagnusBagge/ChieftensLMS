(function () {

	angular.module("app").controller("courseMineController", function ($scope, ApiService) {

		$scope.apiData = {};

		var onSuccess = function (data) {
			$scope.apiData.courses = data.Courses;
		};

		var onFail = function (response) {
			alert(response.Reason);
		};

		ApiService.Get("/CourseApi/Mine", onSuccess, onFail);

		$scope.$on("courseMineController_refresh", function () {
			ApiService.Get("/CourseApi/Mine", onSuccess, onFail);
		});
	});

}());