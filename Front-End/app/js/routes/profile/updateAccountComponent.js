(function(){
	function updateAccountController($scope, userService){
		var self = this;
		this.$routerOnActivate = function(next, previous) {
            this.name = next.params.name;
            this.credential = userService.getCredentialInfo();
        };
		this.doUpdate = function(){
			var form = $scope.accountForm;
            form.$setSubmitted();
            for (var key in form) {
                //console.log(form[key]);
                if (form.hasOwnProperty(key) && form[key] && form[key].$touched !== undefined) {
                    form[key].$setTouched();
                }
            }
            if (form.$valid) {
				userService.changePassword({
					UserLoginID: this.credential.UserLoginID,
					Password: self.NewPassword,
					OldPassword: self.OldPassword
				}).then(() => {
					toastr.success('Update successfully!', 'Success :)');
				}).catch((err) => {
                    if(err.data && err.data.code){
                        $translate(`error.${err.data.code}`).then(msg => toastr.error(msg, 'Failure :('));
                    } else {
                        toastr.error('Failed to update!', 'Failure :(');
                    }
                });;
			}
		}
		self.validateConfirmPassword = function () {
            if (self.NewPassword !== self.ConfirmPassword) {
                $scope.accountForm['confirmPasswordInput'].$setValidity('mismatched', false);
            } else {
                $scope.accountForm['confirmPasswordInput'].$setValidity('mismatched', true);
            }
        };
	}
	angular.module("angularApp.routes").component("updateAccount",{
		templateUrl: "routes/profile/account.html",
		controller: updateAccountController
	});
})();