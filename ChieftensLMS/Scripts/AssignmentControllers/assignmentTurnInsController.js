(function () {

	angular.module("app").controller("assignmentTurnInsController", function ($scope, ApiService) {
		$scope.assignmentFiles = null;
		$scope.id;

		var onFileSuccess = function (data) {
			$scope.assignmentFiles = data.assignmentFiles;
		};

		var onFileFail = function (response) {
			alert("Misslyckades ladda filer");
		};

		$scope.getAssignment = function () 
		{
			ApiService.Get("/AssignmentApi/FilesForAssignment", onFileSuccess, onFileFail, { id: $scope.id });
		}

		var onDeleteSuccess = function (data) {
			var index = -1;
			for (var i = 0; i < $scope.assignmentFiles.length; i++) {
				if ($scope.assignmentFiles[i].Id == data.Id) {
					index = i;
					break;
				}
			}

			if (index != -1)
				$scope.assignmentFiles.splice(index, 1);

		};

		var onDeleteFail = function (response) {
			alert("fail " + response.reason);
		};

		$scope.deleteAssignmentFile = function (id) {
			ApiService.Get("/AssignmentApi/DeleteTurnIn", onDeleteSuccess, onDeleteFail, { id: id });
		}

		$scope.$on("assignmentTurnInsController_refresh", function () {
			$scope.getAssignment();
		})
	});

}());