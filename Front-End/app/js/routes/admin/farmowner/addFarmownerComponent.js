(function() {
    function addFarmownerController($scope, userService) {
        var self = this;
        self.toggleFarmownerAdd = function(){
            self.isShowFarmownerAdd = !self.isShowFarmownerAdd;
        }
        self.doAddFarmowner = function(){
            userService.createLoginCredential({
                    LoginCredential: {
                        UserLoginID: self.newFarmowner.UserPhoneNumber,
                        Password: Math.random().toString(36).substring(9,17)
                    },
                    User: self.newFarmowner,
                    RegisterType: "UserPhoneNumber",
                    UserType: "Farm Owner"
            }).then(() => {
                toastr.success('Add successfully!', 'Success :)');
            });
        }
    }
    angular.module("angularApp.routes").component("addFarmowner", {
        templateUrl: "routes/admin/farmowner/add.html",
        controller: addFarmownerController
    });
})();