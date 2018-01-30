(function(){
	function viewProfileController(userService){
		this.$routerOnActivate = function(next, previous) {
			console.log("inside");
            this.name = next.params.name;
            this.credential = userService.getCredentialInfo();
        };
	}
	angular.module("angularApp.routes").component("viewProfile",{
		templateUrl: "routes/profile/view.html",
		controller: viewProfileController
	});
})();