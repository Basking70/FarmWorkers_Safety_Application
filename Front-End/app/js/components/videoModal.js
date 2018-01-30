(function(){
	function videoModalController($scope, close, video){
		$scope.video = video;
		$scope.dismissModal = function(result) {
		    close(result, 200); // close, but give 200ms for bootstrap to animate
		};

	}
	angular.module("angularApp.components").controller("videoModal",videoModalController);
})();