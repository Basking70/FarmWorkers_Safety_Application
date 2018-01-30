(function(){
	function farmSelectListComponent($scope, farmService){
		var self = this;
		this.$onInit = function(){
			var initNgModal = self.ngModel;
			farmService.listFarms().then(function(farms){
				self.farms = farms;
				self.selectedFarm = self.farms.find(x => x.FarmID === initNgModal);
				self.ngModal = initNgModal;
				self.outFarmModel = self.selectedFarm;
			});
		}
		$scope.$watch(function(){
			return self.selectedFarm;
		},function(newValue){
			if(newValue){
				self.outFarmModel = newValue;
				self.ngModel = newValue ? newValue.FarmID : undefined;
			}
		});
	}
	angular.module("angularApp.components").component("farmSelectList",{
		templateUrl: "components/farmSelectList.html",
		controller: farmSelectListComponent,
		bindings:{
			ngModel: '=',
			outFarmModel: '='
		}
	});
})();