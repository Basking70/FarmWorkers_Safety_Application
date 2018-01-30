angular.module("angularApp",
	['ngComponentRouter',
	'angularApp.components',
	'angularApp.routes', 
	'angularApp.services',
	'angularApp.directives',
	'angularApp.filters',
	'angularApp.constants',
	'angularApp.models',
	'ui.bootstrap',
	'pascalprecht.translate',
	'ngTable',
	'angularModalService',
	'datePicker'])
.config(function($locationProvider) {
  $locationProvider.html5Mode(true);
})
.value('$routerRootComponent', 'root');
angular.module("angularApp.components",[]);
angular.module("angularApp.routes",[]);
angular.module("angularApp.services",[]);
angular.module("angularApp.directives",[]);
angular.module("angularApp.filters",[]);
angular.module("angularApp.constants",[]);
angular.module("angularApp.models",[]);

toastr.options = {
  "positionClass": "toast-top-center"
};