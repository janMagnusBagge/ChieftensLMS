(function () {

	angular.module("app").controller("courseSingleTeacherController", function ($scope, ApiService) {

		$scope.apiData = {};
		var originalName;
		var originalDescription;

		var onSuccess = function (data) {
			$scope.apiData = data;
		};

		var onFail = function (response) {
			alert(response.Reason);
			$scope.apiData.Name = originalName;
			$scope.apiData.Description = originalDescription;
		};

		var onEditSuccess = function (data) {

		};

		var onEditFail = function (response) {
			alert(response.Reason);

		};



		$scope.enableEditMode = function () {
			$scope.editMode = true;
			originalName = $scope.apiData.Name;
			originalDescription = $scope.apiData.Description;
		};

		$scope.saveChanges = function (id) {
			if ($scope.apiData.Name != originalName || $scope.apiData.Description != originalDescription)
			{
				ApiService.Post("/CourseApi/EditCourse", onEditSuccess, onEditFail, {courseId: id, name: $scope.apiData.Name, description: $scope.apiData.Description});
				$scope.editMode = false;
			}
			else
			{
				$scope.cancelEdit();
			}
		}

		$scope.cancelEdit = function () {
			$scope.apiData.Name = originalName;
			$scope.apiData.Description = originalDescription;
			$scope.editMode = false;
		};

		$scope.getSingle = function (id) {
			ApiService.Get("/CourseApi/Single", onSuccess, onFail, { id: id });
		};

	});

}());