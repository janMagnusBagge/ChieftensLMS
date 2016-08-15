(function () {

	/*
		Angular service for API communication.
		Currently we are not handling when requests fail, so for that the dummyFailReason will be returned to all callbacks.
	*/
	angular.module("app").factory('ApiService', function ($http) {
		// Dummy placeholder, we dont handle errors atm so this is sent to fail callbacks
		var dummyFailReason = { reason: "Hey there" };

		// Helper function for generating API functions, requires config.url/config.method. The config parameter is the same as the one that's passed into $http:
		// https://docs.angularjs.org/api/ng/service/$http
		//
		// Function returned depends on the config.method:
		// 'PUT', 'POST', 'PATCH': function(successsCallback, failCallback, data)
		// 'GET': function(successsCallback, failCallback, [params])
		// All others: function(successsCallback, failCallback)
		function generateApiFunction(config) {
			if (config.method == 'PUT' || config.method == 'POST' || config.method == 'PATCH') {
				return function (successsCallback, failCallback, data) // PUT, POST, PATCH return this function
				{
					delete config.params;
					config.data = data;
					$http(config).then(
					function (response) {
						if (response.data.success == true)
							successsCallback(response.data.data);
						else
							failCallback(dummyFailReason);
					},
					function (response) {
						failCallback(dummyFailReason);
					}
					);
				}
			}
			else if (config.method == 'GET') {
				return function (successsCallback, failCallback, params) { // GET returns this function, params are optional
					delete config.data;

					if (typeof params !== 'undefined')
						config.params = params;
					else
						delete config.params;

					$http(config).then(
					function (response) {
						if (response.data.success == true)
							successsCallback(response.data.data);
						else
							failCallback(dummyFailReason);
					},
					function (response) {
						failCallback(dummyFailReason);
					});
				}
			}
			else {
				return function (successsCallback, failCallback) // The other http methods return this
				{

					delete config.data;
					delete config.params;

					$http(config).then(
					function (response) {
						if (response.data.success == true)
							successsCallback(response.data.data);
						else
							failCallback(dummyFailReason);
					},
					function (response) {
						failCallback(dummyFailReason);
					});
				}
			}
		}

		// Generate the api functions using helper method
		var GetVehicleTypes = generateApiFunction({ method: 'GET', url: '/API/GetVehicleTypes' });
		var GetList = generateApiFunction({ method: 'GET', url: '/API/GetList' });
		var GetSimpleList = generateApiFunction({ method: 'GET', url: '/API/GetSimpleList' });
		var GetSingle = generateApiFunction({ method: 'GET', url: '/API/GetSingle' });
		var Edit = generateApiFunction({ method: 'GET', url: '/API/Edit' });
		var Remove = generateApiFunction({ method: 'GET', url: '/API/Remove' });
		var Add = generateApiFunction({ method: 'POST', url: '/API/Add' });


		var GetAllCourses = generateApiFunction({ method: 'GET', url: '/CourseApi/Index' });

		// The service with all the API call functions
		return {
			GetAllCourses: GetAllCourses,

			GetVehicleTypes: GetVehicleTypes,
			GetList: GetList,
			GetSingle: GetSingle,
			Edit: Edit,
			GetSimpleList: GetSimpleList,
			Add: Add,
			Remove: Remove
		};
	});


}());