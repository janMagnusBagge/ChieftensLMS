(function () {

	angular.module("app").controller("courseCreateCourseController", function ($scope, ApiService) {

		var onAddSuccess = function (data) {
			$scope.IsCreated = true;
			$scope.CreatedCourseId = data.CourseId;
		};

		var onAddFail = function (response) {
			alert(response.Reason);
		};

		$scope.CreateCourse = function () {
			$scope.form.Name.$touched = true;
			$scope.form.Description.$touched = true;

			if ($scope.form.$valid) {
				ApiService.Get("/CourseApi/CreateCourse", onAddSuccess, onAddFail, { name: $scope.Name, description: $scope.Description });
			}
		};



	});

}());