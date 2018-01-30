(function(){
	function roleVisibleDirective(userService){
		return {
			restrict: 'A',
			scope: {
            	roleVisible: '='
        	},
			link: function($scope, element, attributes) {
                if($scope.roleVisible.indexOf(userService.getCurrentRole()) < 0){
                	element.hide();
                }
        	}
		}
	}
	angular.module("angularApp.directives").directive("roleVisible",roleVisibleDirective);
})();