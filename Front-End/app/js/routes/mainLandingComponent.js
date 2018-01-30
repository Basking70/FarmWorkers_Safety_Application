(function(){
	function mainLandingController(userService){
		this.$routerOnActivate = function(next, previous) {
            this.name = next.params.name;
            this.credential = userService.getCredentialInfo();
        };
	}
	angular.module("angularApp.routes").component("mainLanding",{
		templateUrl: "routes/mainLanding.html",
		controller: mainLandingController
	});
})();