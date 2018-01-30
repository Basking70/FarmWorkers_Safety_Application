(function(){
	function UserFarmFactory(Farm){
		function UserFarm(obj = {}){
			var json = obj ? obj : {};
			this.UserInFarmID = json.UserInFarmID;
            this.FarmID = json.FarmID;
            this.Farm = new Farm(json.Farm);
            this.UserID = json.UserID;
			this.IsLatest = json.IsLatest;
		}
		return UserFarm;
	}
	angular.module("angularApp.models").factory("UserFarm", UserFarmFactory);
})();