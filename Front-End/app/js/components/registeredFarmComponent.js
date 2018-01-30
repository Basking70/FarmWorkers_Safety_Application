(function(){
	function registeredFarmComponent($scope, farmService){
	}
	angular.module("angularApp.components").component("registeredFarm",{
		templateUrl: "components/registeredFarm.html",
		controller: registeredFarmComponent,
		bindings:{
			currentFarm: '<'
		}
	});
})();