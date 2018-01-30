(function(){
	function farmService($http,$q, $timeout, API_CONSTANT, Farm){
		var self = this;
		var farmsPromiseCache;
		self.listFarms = function(){
			if(!farmsPromiseCache){
				farmsPromiseCache = $http.get(API_CONSTANT.API_HOST + API_CONSTANT.API.ListFarm);
			}
			return farmsPromiseCache
			.then((resp) =>  resp.data.map(farm => new Farm(farm)))
			.finally(() => {
				farmsPromiseCache = undefined;	
			});

		}
		self.addFarm = function(farm){
			return $http.post(API_CONSTANT.API_HOST + API_CONSTANT.API.InsertFarm, farm);
		};

		self.updateFarm = function(farm){
			return $http.post(API_CONSTANT.API_HOST + API_CONSTANT.API.UpdateFarm, farm);
		}
		self.deleteFarm = function(FarmID){
			return $http.post(API_CONSTANT.API_HOST + API_CONSTANT.API.DeleteFarm, { FarmID: FarmID });

			console.log(FarmID);
		}
	}
	angular.module("angularApp.services").service("farmService",farmService);
})();
