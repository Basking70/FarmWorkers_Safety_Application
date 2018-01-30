(function() {
    function addFarmworkerController($scope, userService) {
        var self = this;
        self.doAddFarmworker = function(){
            userService.createLoginCredential({
                    LoginCredential: {
                        UserLoginID: self.newFarmworker.UserPhoneNumber,
                        Password: Math.random().toString(36).substring(9,17)
                    },
                    User: self.newFarmworker,
                    RegisterType: "UserPhoneNumber",
                    UserType: "Farm Worker"
            }).then(() => {
                toastr.success('Add successfully!', 'Success :)');
            });
        }
    }
    angular.module("angularApp.routes").component("addFarmworker", {
        templateUrl: "routes/admin/farmworker/add.html",
        controller: addFarmworkerController
    });
})();