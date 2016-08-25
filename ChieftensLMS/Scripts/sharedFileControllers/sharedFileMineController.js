
	(function () {

		angular.module("app").controller("sharedFileMineController", function ($scope, ApiService) {
			console.log($scope);
			$scope.apiData = {};

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


			ApiService.Get("/SharedFileApi/Mine", onSuccess, onFail);

			

			$scope.Delete = function (id) {

				ApiService.Get("/SharedFileApi/Delete", onDeleteSuccess, onDeleteFail, { id: id });
			}

			$scope.test = function () {
				$state.reload();
			}

		});

	}());
