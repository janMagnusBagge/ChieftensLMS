(function () {
	var app = angular.module('app', ['ngFileUpload']);

	// Filter to convert date format from server to js
	app.filter("dateFilter", function () {
		return function (item) {
			if (item != null) {
				return new Date(parseInt(item.substr(6)));
			}
			return "";
		}
	});

}());

