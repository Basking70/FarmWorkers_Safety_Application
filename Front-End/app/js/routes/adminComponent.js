(function() {
    function adminController($scope) {
        var self = this;
        self.isActive = function(childRouteName) {
		    var instruction = self.$router.generate(childRouteName);
		    return self.$router.isRouteActive(instruction);
	  	}
    }
    angular.module("angularApp.routes").component("admin", {
        templateUrl: "routes/admin.html",
        controller: adminController,
        $routeConfig : [
		{
            path: '/',
            component: 'adminLanding',
            name: 'AdminLanding',
            useAsDefault: true
        },{
            path: '/farmworker/...',
            component: 'farmworkerAdmin',
            name: 'FarmworkerAdmin'
        },{
            path: '/farmowner/...',
            component: 'farmownerAdmin',
            name: 'FarmownerAdmin'
        },
        {
            path: '/education/...',
            component: 'educationAdmin',
            name: 'EducationAdmin'
        },{
            path: '/farm',
            component: 'farmAdmin',
            name: 'FarmAdmin'
        },{
            path: '/security',
            component: 'securityAdmin',
            name: 'SecurityAdmin'
        }],
        bindings: {
			$router : '<'
		}
    });
})();