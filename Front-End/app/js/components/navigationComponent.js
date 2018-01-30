(function(){
	function navigationController(userService, $rootRouter){

		var self = this;
		self.doLogout = function(){
			userService.logout().then(function(){
				$rootRouter.navigate(["Landing"]);
			});
		}
	}
	function resolveTemplate(tElement, tAttrs) {

}
	angular.module("angularApp.components").component("navigation",{
		templateUrl: "components/navigation.html",
		controller: navigationController,
		bindings: {
			$router : '<'
		}
	});
})();