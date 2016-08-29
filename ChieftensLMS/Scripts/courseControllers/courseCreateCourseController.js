(function () {

	angular.module("app").controller("courseCreateCourseController", function ($scope, ApiService) {

		var onAddSuccess = function (data) {
			$scope.IsCreated = true;
			$scope.CreatedCourseId = data.CourseId;
			$scope.$emit("courseCreateCourseController_created");
		};

		var onAddFail = function (response) {
			alert(response.Reason);
		};

		$scope.CreateCourse = function () {
			if ($scope.form.$valid == false) {
				$scope.form.Name.$setTouched();
				$scope.form.Description.$setTouched();
				return;
			}
		
				ApiService.Get("/CourseApi/CreateCourse", onAddSuccess, onAddFail, { name: $scope.Name, description: $scope.Description });
		
		};
	
		$scope.reset = function () {
			$scope.IsCreated = false;
			$scope.form.$setUntouched();
			$scope.Name = undefined;
			$scope.Description = undefined;
			$scope.CreatedCourseId = undefined;
		};

	});

}());