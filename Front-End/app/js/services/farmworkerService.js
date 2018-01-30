(function(){
	function farmworkerService($http,$q, $timeout, API_CONSTANT, User){
		var workers = [{
		        	Id: 1,
		        	Email: "johndoe@farmworkers.com",
					FirstName: "John",
					LastName: "Doe",
					Phone: "3102139009",
					RegisteredFarm: {
						Name: "John's Marvelous Farm",
						Address: "2638 Bevelry Hills, Los Angeles, CA, 90007"
					}
				},
				{
		        	Id: 2,
		        	Email: undefined,
					FirstName: "Kayne",
					LastName: "West",
					Phone: "2301039555",
					RegisteredFarm: {
						Name: "Not the East Farm",
						Address: "2638 Francisco, San Francisco, CA, 32000"
					}
				},{
		        	Id: 3,
		        	Email: "thenry@bbc.co.uk",
					FirstName: "Thierry",
					LastName: "Henry",
					Phone: "4441231234",
					RegisteredFarm: {
						Name: "Le Coq Farm",
						Address: "11 Watt Ways, Los Angeles, CA, 90007"
					}
				},
				{
		        	Id: 4,
		        	Email: "dbeckham@manutd.com",
					FirstName: "David",
					LastName: "Beckham",
					Phone: "5881129999",
					RegisteredFarm: {
						Name: "Old Trafform Pig's Farm",
						Address: "99 Figueroa St, Los Angeles, CA, 90007"
					}
				}
	        ];
			this.searchFarmworkers = function(email,phone,name){
				return $http.get(API_CONSTANT.API_HOST + API_CONSTANT.API.SearchUser,{
					params: {
						UserType: 'Farm Worker',
						email: email,
						phone: phone,
						name: name
					}
				}).then((resp) => resp.data.map(x=> new User(x)));
			}
			this.updateFarmworker = function(user){
				return $http.post(API_CONSTANT.API_HOST + API_CONSTANT.API.UpdateUser, user).then((resp) => new User(resp.data));
			}
		this.searchFarmworkers2 = function(email,phone,name){
			console.log(!!!phone);
			console.log(!!!name);
			var filtered = [];
			filtered = workers.filter(function(worker){
				if(!!!email) return true;
				if( !worker.Email ) return false;
				else{
					return worker.Email.indexOf(email) >= 0;
				}
			});
			console.log(filtered);
			filtered = filtered.filter(function(worker){
				if(!!!phone) return true;
				if( !worker.Phone ) return false;
				else{
					console.log(worker.Phone);
					return worker.Phone.indexOf(phone) >= 0;
				}
			});
			console.log(filtered);
			filtered = filtered.filter(function(worker){
				if(!!!name) return true;
				if( !worker.FirstName && !worker.LastName) return false;
				else{
					return (worker.FirstName ? worker.FirstName.indexOf(name) >= 0 : false) || (worker.LastName ? worker.LastName.indexOf(name) >= 0 : false) ;
				}
			});
			return filtered;
		};
	}
	angular.module("angularApp.services").service("farmworkerService",farmworkerService);
})();