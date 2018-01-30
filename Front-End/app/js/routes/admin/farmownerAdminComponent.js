(function() {
    function farmownerAdminController($scope, farmownerService, userService, NgTableParams) {
      var self = this;
        self.isActive = function(childRouteName) {
		    var instruction = self.$router.generate(childRouteName);
		    return self.$router.isRouteActive(instruction);
	  	}
    }
    angular.module("angularApp.routes").component("farmownerAdmin", {
        templateUrl: "routes/admin/farmowner.html",
        controller: farmownerAdminController,
        $routeConfig : [
		{
            path: '/add',
            component: 'addFarmowner',
            name: 'AddFarmowner',
            
        },
        {
            path: '/search',
            component: 'searchFarmowner',
            name: 'SearchFarmowner',
            useAsDefault: true
        }],
        bindings: {
			$router : '<'
		}
    });
})();