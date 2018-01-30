(function(){
	function FarmFactory(){
		function Farm(obj = {}){
			var json = obj ? obj : {};
			this.FarmID  = json.FarmID ;
			this.FarmName  = json.FarmName;
			this.FarmHouseNumberStreetAddress = json.FarmHouseNumberStreetAddress;
			this.FarmCity = json.FarmCity;
			this.FarmState = json.FarmState;
			this.FarmCountry = json.FarmCountry;
			this.FarmZipCode = json.FarmZipCode;
			this.FarmLatitude = json.FarmLatitude;
			this.FarmLongitude = json.FarmLongitude;
			this.FarmTemperatureMin = json.FarmTemperatureMin;
			this.FarmTemperatureMax = json.FarmTemperatureMax;
			this.IsActive = json.IsActive;
			this.FarmOwners = json.FarmOwners;
			this.NumberOfFarmWorkers = json.NumberOfFarmWorkers;	

			Object.defineProperty(this, 'FarmOwnersName', { 
                                get: () => { 
                                        return this.FarmOwners.map(x=> x.UserName + " " + x.UserLastName).join(", ");
                                } 
                        });
			}
		return Farm;
	}
	angular.module("angularApp.models").factory("Farm", FarmFactory);
})();