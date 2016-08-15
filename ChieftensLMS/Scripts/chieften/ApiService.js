(function () {

	/*
		Angular service for API communication.
		Currently we are not handling when requests fail, so for that the dummyFailReason will be returned to all callbacks.
	*/
	angular.module("app").factory('ApiService', function ($http) {
		// Dummy placeholder, we dont handle errors atm so this is sent to fail callbacks
		var dummyFailReason = { reason: "Hey there" };


		var debugCall = function (config, response, data) {
			console.log("API CALL: " + config.method + " " + config.url + " " + JSON.stringify(config.params));

			if (typeof (data) !== 'undefined')
			{
				console.log("WITH DATA: ");
				console.log(config.data);
			}

			console.log("RESPONSE:");
			console.log(response);
		}

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
						debugCall(config, response);
						if (response.data.success == true)
							successsCallback(response.data.data);
						else
							failCallback(dummyFailReason);
					},
					function (response) {
						debugCall(config, response);
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

						debugCall(config, response);

						if (response.data.success == true)
							successsCallback(response.data.data);
						else
							failCallback(dummyFailReason);
					},
					function (response) {
						debugCall(config, response);

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
						debugCall(config, response);
						if (response.data.success == true)
							successsCallback(response.data.data);
						else
							failCallback(dummyFailReason);
					},
					function (response) {
						debugCall(config, response);
						failCallback(dummyFailReason);
					});
				}
			}
		}

		// Generate the api functions using helper method
		var Remove = generateApiFunction({ method: 'GET', url: '/API/Remove' });
		var Add = generateApiFunction({ method: 'POST', url: '/API/Add' });


		var GetAllCourses = generateApiFunction({ method: 'GET', url: '/CourseApi/Index' });
		var GetCourse = generateApiFunction({ method: 'GET', url: '/CourseApi/Details' });

		var GetSharedFilesForCourse = generateApiFunction({ method: 'GET', url: '/SharedFileApi/ForCourse' });

		var GetCourseAssigments = generateApiFunction({ method: 'GET', url: '/AssigmentApi/GetAssignmentForCourse' });
		var GetAssigment = generateApiFunction({ method: 'GET', url: '/AssigmentApi/GetAssigment' });
		

		// The service with all the API call functions
		return {
			GetAllCourses: GetAllCourses,
			GetCourse: GetCourse,
			GetSharedFilesForCourse: GetSharedFilesForCourse,
			GetCourseAssigments: GetCourseAssigments,
			GetAssigment: GetAssigment,
			Add: Add,
			Remove: Remove
		};
	});


}());