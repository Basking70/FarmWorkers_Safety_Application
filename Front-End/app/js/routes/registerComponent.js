(function () {
    function registerController($scope, userService, $translate) {
        var self = this;
        self.months = ["January", "Febuary"];
        self.days = [1, 2, 3, 4, 5];
        self.years = [1990, 1991, 1992];
        self.isSubmitted = false;
        self.registerUser = {
            IsActive: 1
        }
        self.doRegister = function () {
            var form = $scope.registerForm;
            form.$setSubmitted();
            for (var key in form) {
                //console.log(form[key]);
                if (form.hasOwnProperty(key) && form[key] && form[key].$touched !== undefined) {
                    form[key].$setTouched();
                }
            }
            if (form.$valid) {
                self.isSubmitted = true;
                userService.register({
                    LoginCredential: self.registerUser,
                    User: {},
                    RegisterType: "UserPhoneNumber",
                    UserType: "Farm Worker"
                }).then(function (credential) {
                    toastr.success('Registerer successfully!', 'Success :)');
                    if (credential.User.UserType === 'Admin') {
                        self.$router.navigate(['Main', 'Admin']);
                    } else {
                        self.$router.navigate(['Main']);
                    }
                }).catch((err) => {
                    if(err.data && err.data.code){
                        $translate(`error.${err.data.code}`).then(msg => toastr.error(msg, 'Failure :('));
                    } else {
                        toastr.error('Failed to register!', 'Failure :(');
                    }
                });
            }
        };

        self.validatePassword = function () {
            var correctFormat = self.registerUser.Password.match(/[0-9a-zA-Z!@#\$%\^\&*\)\(+=._-]{6,24}/g);
            var hasUpper = self.registerUser.Password.match(/[A-Z]+/g);
            var hasSpecial = self.registerUser.Password.match(/[!@#\$%\^\&*\)\(+=._-]+/g);
            if (!correctFormat || !hasUpper || !hasSpecial) {
                $scope.registerForm['passwordInput'].$setValidity('pattern', false);
            } else {
                $scope.registerForm['passwordInput'].$setValidity('pattern', true);
            }
        }
        self.validateConfirmPassword = function () {
            console.log("called")
            if (self.registerUser.Password !== self.registerUser.ConfirmPassword) {
                console.log("failed");
                $scope.registerForm['confirmPasswordInput'].$setValidity('mismatched', false);
            } else {
                $scope.registerForm['confirmPasswordInput'].$setValidity('mismatched', true);
            }
        };
        $scope.$watch(function () {
            return $scope.registerForm.$dirty;
        }, function (newValue) {
            if (newValue) {
                self.isSubmitted = false;
            }
        })
    }
    angular.module("angularApp.routes").component("register", {
        templateUrl: "routes/register.html",
        bindings: {
            $router: '<'
        },
        controller: registerController
    });
})();