(function(){
	function farmownerService($http,$q, $timeout, API_CONSTANT, User){
		this.searchFarmowners = function(email,phone,name){
			return $http.get(API_CONSTANT.API_HOST + API_CONSTANT.API.SearchUser,{
				params: {
					UserType: 'Farm Owner',
					email: email,
					phone: phone,
					name: name
				}
			}).then((resp) => resp.data.map(x=> new User(x)));
		};
		this.updateFarmowner = function(user){
				return $http.post(API_CONSTANT.API_HOST + API_CONSTANT.API.UpdateUser, user).then((resp) => new User(resp.data));
		}
	}
	angular.module("angularApp.services").service("farmownerService",farmownerService);
})();