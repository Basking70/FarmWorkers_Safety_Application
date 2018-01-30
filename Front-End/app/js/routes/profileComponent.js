(function(){
	function profileController(userService){
        var self = this;
		this.$routerOnActivate = function(next, previous) {
            this.name = next.params.name;
            this.user = userService.getUserInfo();
        };
        self.isActive = function(childRouteName) {
		    var instruction = self.$router.generate(childRouteName);
		    return self.$router.isRouteActive(instruction);
	  	}
	}
	angular.module("angularApp.routes").component("profile",{
		templateUrl: "routes/profile.html",
		controller: profileController,
		$routeConfig : [
        {
            path: '/profile',
            component: 'updateProfile',
            name: 'UpdateProfile',
            useAsDefault: true
        },
        {
            path: '/account',
            component: 'updateAccount',
            name: 'UpdateAccount'
        },
        {
            path: '/farm',
            component: 'updateFarm',
            name: 'UpdateFarm'
        },{
            path: '/communicationPreference',
            component: 'updateCommunicationPreference',
            name: 'UpdateCommunicationPreference'
        }],
        bindings: {
			$router : '<'
		}
	});
})();