(function () {
	var app = angular.module('app', ['ngFileUpload']);

	app.controller('sharedFileUploadController', function ($scope, Upload) {

		$scope.uploadStatus = '';

		$scope.upload = function (file, courseId) {

			if ($scope.form.$valid == false){
				$scope.form.Name.$touched = true;
				return;
			}

			$scope.uploadStatus = 'uploading';

			Upload.upload({
				url: '/SharedFileApi/Upload/',
				data: { file: file, name: $scope.Name, fileName: file.name, courseId: courseId }
			}).then(function (resp) {
				console.log(resp);
				$scope.uploadStatus = 'complete';
			}, function (resp) {
				console.log(resp);
				$scope.uploadStatus = 'error';
			}, function (evt) {
				$scope.progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
				$scope.maxProgress = evt.total;
			});

		};


	});

}());