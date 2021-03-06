﻿
(function () {
	angular.module("app").controller("sharedFileForCourseController", function ($scope, ApiService) {
		$scope.apiData = {};
		$scope.id;

		var onSuccess = function (data) {
			$scope.apiData = data;
		};

		var onFail = function (response) {
			alert(response.Reason);
		};

		var onDeleteSuccess = function (data) {
			var index = -1;
			for (var i = 0; i < $scope.apiData.SharedFiles.length; i++) {
				if ($scope.apiData.SharedFiles[i].Id == data.Id) {
					index = i;
					break;
				}
			}

			if (index != -1)
				$scope.apiData.SharedFiles.splice(index, 1);

		};

		var onDeleteFail = function (response) {
			alert(response.Reason);
		};

		$scope.GetForCourse = function () {
			ApiService.Get("/SharedFileApi/ForCourse", onSuccess, onFail, { id: $scope.id });
		}

		$scope.Delete = function (id) {
			ApiService.Get("/SharedFileApi/Delete", onDeleteSuccess, onDeleteFail, { id: id });
		}

		$scope.$on("sharedFileForCourseController_refresh", function () {
			$scope.GetForCourse();
		})

	});

}());
