(function() {
    function farmworkerAdminController($scope, farmworkerService, userService, NgTableParams) {
      var self = this;
        self.isActive = function(childRouteName) {
		    var instruction = self.$router.generate(childRouteName);
		    return self.$router.isRouteActive(instruction);
	  	}
    }
    angular.module("angularApp.routes").component("farmworkerAdmin", {
        templateUrl: "routes/admin/farmworker.html",
        controller: farmworkerAdminController,
        $routeConfig : [
		{
            path: '/add',
            component: 'addFarmworker',
            name: 'AddFarmworker',
            
        },
        {
            path: '/search',
            component: 'searchFarmworker',
            name: 'SearchFarmworker',
            useAsDefault: true
        }],
        bindings: {
			$router : '<'
		}
    });
})();