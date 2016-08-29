(function () {

	angular.module("app").controller('sharedFileUploadController', function ($scope, Upload) {

		$scope.uploadStatus = '';
		
		$scope.upload = function (file, courseId) {
			
			if ($scope.form.$valid == false){
				$scope.form.Name.$setTouched();
				$scope.form.File.$setTouched();
				return;
			}

			$scope.uploadStatus = 'uploading';

			Upload.upload({
				url: '/SharedFileApi/Upload/',
				data: { file: file, name: $scope.Name, fileName: file.name, courseId: courseId }
			}).then(function (resp) {
				console.log(resp);
				$scope.uploadStatus = 'complete';
				$scope.$emit("sharedFileUploadController_uploadComplete");
			}, function (resp) {
				console.log(resp);
				$scope.uploadStatus = 'error';
			}, function (evt) {
				$scope.progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
				$scope.maxProgress = evt.total;
			});

		};
		
		$scope.reset = function () {
			$scope.uploadStatus = '';

			$scope.form.$setUntouched();
			$scope.Name = undefined;
			$scope.File = undefined;
		
		}


	});

}());