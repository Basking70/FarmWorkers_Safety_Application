(function() {
    function farmAdminController($scope ,farmService) {
        var self = this ;

        this.$onInit = function(){
            console.log("load farms from backend");
            this.initComponent();
        }
        this.initComponent = function(){
            farmService.listFarms().then(function(farms){
                console.log(farms);
                $scope.farms = farms;
            });
        }
        // creating attributes 
        $scope.FarmID = '';
        $scope.FarmName = '';
        $scope.FarmOwner = '';
        $scope.FarmHouseNumberStreetAddress = '';
        $scope.FarmCity = '';
        $scope.FarmState = '';
        $scope.FarmCountry = '';
        $scope.FarmZipCode = '';
        $scope.fnum = '';
        
        /// make them as defult 
        $scope.FarmLatitute = '33.983681'; //Dummy Data
        $scope.FarmLongitude = '-118.338542'; //Dummy Data
        $scope.FarmTemperatureMin = '1'; //Dummy Data
        $scope.FarmTemperatureMax = '100'; //Dummy Data
        $scope.IsActive = '1';
        $scope.FarmWeather = 'null';
        
        // page defaults 
        $scope.error = false;
        $scope.incomplete = false;
        $scope.hideform = true;
        
        // canceling a new farm
        $scope.CancelFarm = function() {
                        $scope.hideform = true;
                    };
        
        // function for adding and editing Farm's information
        $scope.editFarm = function(FarmID) {
            $scope.hideform = false;
			$scope.createTitle = true;
			$scope.editTitle = false;
			$scope.edit = true;
			$scope.errorMessages = true;
			
			
			
            if (FarmID == 'new') {
				
				$scope.createTitle = false;
				$scope.editTitle = true;
				$scope.errorMessages = true;
				
                // adding a new farm
                $scope.FarmName = '';
                $scope.FarmOwner = '';
                $scope.FarmHouseNumberStreetAddress = '';
                $scope.FarmCity = '';
                $scope.FarmState = '';
                $scope.FarmCountry = '';
                $scope.FarmZipCode = '';
                // $scope.FarmLatitute = '';
                // $scope.FarmLongitude = '';
                // $scope.FarmTemperatureMin = '';
                // $scope.FarmTemperatureMax = '';
                // $scope.IsActive = '1';
                // $scope.FarmWeather = 'null';
				
                // seving a new farm
                $scope.saveFarm = function() {;
                    var newFarm = {
                        'FarmName': $scope.FarmName,
                        'FarmOwner': $scope.FarmOwner,
                        'FarmHouseNumberStreetAddress': $scope.FarmHouseNumberStreetAddress,
                        'FarmCity': $scope.FarmCity,
                        'FarmState': $scope.FarmState,
                        'FarmCountry': $scope.FarmCountry,
                        'FarmZipCode': $scope.FarmZipCode,
                        'FarmLatitute': $scope.FarmLatitute , 
                        'FarmLongitude': $scope.FarmLongitude , 
                        'FarmTemperatureMin': $scope.FarmTemperatureMin , 
                        'FarmTemperatureMax':  $scope.FarmTemperatureMax , 
                        'IsActive': $scope.IsActive ,
                        'FarmWeather': $scope.FarmWeather
                    };

					
					farmService.addFarm(newFarm).then(
									function(resp){ 
										console.log(resp.data);
										
										if(typeof resp.data === 'string'){
											
											$scope.errorMessages = false;
											$scope.messages  = resp.data.toString().split("<br>");
										}
										else
										{
											$scope.farms.push(newFarm);
											self.initComponent;
											$scope.hideform = true;
										}
									}
								);
					
                }
            } else {
                // editing a new farm
                var index = -1;
                var comArr = $scope.farms;
                for (var i = 0; i < comArr.length; i++) {
                    if (comArr[i].FarmID === FarmID) {
                    index = i;
                    break;
                    }
                }
                if (index === -1) {
                    alert("Something gone wrong");
                }
            
                $scope.FarmName = $scope.farms[index].FarmName;
                $scope.FarmOwner = $scope.farms[index].FarmOwner;
                $scope.FarmHouseNumberStreetAddress = $scope.farms[index].FarmHouseNumberStreetAddress;
                $scope.FarmCity = $scope.farms[index].FarmCity;
                $scope.FarmState = $scope.farms[index].FarmState;
                $scope.FarmCountry = $scope.farms[index].FarmCountry;
                $scope.FarmZipCode = $scope.farms[index].FarmZipCode;
				
                $scope.saveFarm = function() {
                    $scope.farms[index].FarmName = $scope.FarmName;
                    $scope.farms[index].FarmOwner = $scope.FarmOwner;
                    $scope.farms[index].FarmHouseNumberStreetAddress = $scope.FarmHouseNumberStreetAddress;
                    $scope.farms[index].FarmCity = $scope.FarmCity;
                    $scope.farms[index].FarmState = $scope.FarmState;
                    $scope.farms[index].FarmCountry = $scope.FarmCountry;
                    $scope.farms[index].FarmZipCode = $scope.FarmZipCode;
                
                // updating farm infornmation
                    farmService.updateFarm({
                        'FarmID': $scope.farms[index].FarmID,
                        'FarmName': $scope.FarmName,
                        'FarmOwner': $scope.FarmOwner,
                        'FarmHouseNumberStreetAddress': $scope.FarmHouseNumberStreetAddress,
                        'FarmCity': $scope.FarmCity,
                        'FarmState': $scope.FarmState,
                        'FarmCountry': $scope.FarmCountry,
                        'FarmZipCode': $scope.FarmZipCode,
                        'FarmLatitute': $scope.FarmLatitute,
                        'FarmLongitude': $scope.FarmLongitude,
                        'FarmTemperatureMin': $scope.FarmTemperatureMin,
                        'FarmTemperatureMax': $scope.FarmTemperatureMax,
                        'IsActive': $scope.IsActive,
                        'FarmWeather': $scope.FarmWeather
                    }).then(
									function(resp){ 
										if(typeof resp.data === 'string'){
											$scope.errorMessages = false;
											$scope.messages  = resp.data.toString().split("<br>");
										}
										else
										{
											self.initComponent;
											$scope.hideform = true;
										}
									}
								);
                }
            
            }
        };

        // removing a farm 
        $scope.removeFarm = function(FarmID) {
            $scope.hideform = true;
			$scope.errorMessages = true;
			
            var index = -1;
            var comArr = $scope.farms;
            for (var i = 0; i < comArr.length; i++) {
                if (comArr[i].FarmID === FarmID) {
                    index = i;
                    break;
                }
            }
            if (index === -1) {
                alert("Something gone wrong");
            }
            $scope.farms.splice(index, 1);
            farmService.deleteFarm(FarmID);
            // farmService_delete.deleteFarm({
            // $scope.farms.splice(index, 1);
            // });
        };

    }
    angular.module("angularApp.routes").component("farmAdmin", {
        templateUrl: "routes/admin/farm.html",
        controller: farmAdminController
    });

})();
