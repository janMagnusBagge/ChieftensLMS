(function () {

	angular.module("app").controller("courseSingleController", function ($scope, ApiService) {

		$scope.apiData = {};

		var onSuccess = function (data) {
			$scope.apiData = data;
		};

		var onFail = function (response) {
			alert(response.Reason);
		};

		$scope.getSingle = function (id) {
			ApiService.Get("/CourseApi/Single", onSuccess, onFail, { id: id });
		}

	});

}());