(function () {
	function updateProfileController($scope, userService) {
		this.$routerOnActivate = function (next, previous) {
			console.log("inside");
			this.name = next.params.name;
			this.credential = userService.getCredentialInfo();
		};
		this.doUpdate = function () {
			var form = $scope.profileForm;
			form.$setSubmitted();
			for (var key in form) {
				//console.log(form[key]);
				if (form.hasOwnProperty(key) && form[key] && form[key].$touched !== undefined) {
					form[key].$setTouched();
				}
			}
			if (form.$valid) {
				userService.updateUser(this.credential.User).then(() => {
					toastr.success('Update successfully!', 'Success :)');
				});
			}
		}
	}
	angular.module("angularApp.routes").component("updateProfile", {
		templateUrl: "routes/profile/profile.html",
		controller: updateProfileController
	});
})();