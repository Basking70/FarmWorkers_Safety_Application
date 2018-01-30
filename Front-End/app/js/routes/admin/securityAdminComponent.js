(function() {
    function securityAdminController($scope) {
        var self = this;
      
    }
    angular.module("angularApp.routes").component("securityAdmin", {
        templateUrl: "routes/admin/security.html",
        controller: securityAdminController
    });
})();