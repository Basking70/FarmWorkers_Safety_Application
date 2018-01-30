(function(){
	function videoModalService(ModalService){
		var self = this;
		self.createModal = function(video){
			return ModalService.showModal({
				templateUrl: "components/videoModal.html",
				controller: "videoModal",
				inputs: {
					video: video
				}
			}).then(function(modal) {
	            modal.element.modal();
	            modal.close.then(function() {
	                //$scope.message = "You said " + result;
	            });
	        });;
		}
	}
	function videoModalController(){

	}
	angular.module("angularApp.services").service("videoModalService",videoModalService);
})();