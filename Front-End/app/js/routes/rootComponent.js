(function() {
    function rootController() {
        this.$onInit = function() {
        };


    }
    angular.module("angularApp.routes").component("root", {
        template: "<ng-outlet></ng-outlet>",
        bindings: {},
        controller: rootController,
        $routeConfig: [{
            path: '/main/...',
            component: 'main',
            name: 'Main'
        },{
           path: '/',
            component: 'landing',
            name: 'Landing',
            useAsDefault: true 
        },
        {
           path: '/register',
            component: 'register',
            name: 'Register'
        }]
    });
})();