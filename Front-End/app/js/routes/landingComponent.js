(function () {
	function landingController($scope, $translate, userService, videoModalService) {
		var self = this;
		this.mode = 'Login';
		this.doLogin = function () {
			var form = $scope.landingForm;
			form.$setSubmitted();
			for (var key in form) {
				//console.log(form[key]);
				if (form.hasOwnProperty(key) && form[key] && form[key].$touched !== undefined) {
					form[key].$setTouched();
				}
			}
			if (form.$valid) {
				userService.login(this.username, this.password).then(function (user) {
					toastr.success('Login successfully!', 'Success :)');
					if (user.User.UserType === 'Admin') {
						self.$router.navigate(['Main', 'Admin']);
					} else {
						self.$router.navigate(['Main', 'MainLanding']);
					}
				}).catch((err) => {
					if(err.data && err.data.code){
                        $translate(`error.${err.data.code}`).then(msg => toastr.error(msg, 'Failure :('));
                    } else {
                        toastr.error('Failed to login!', 'Failure :(');
                    }
				});
			}
		};
		this.goToRegister = function () {
			this.$router.navigate(['Register']);
		};

		this.$routerOnActivate = function (next, previous) {
			this.name = next.params.name;
		};
		this.changeMode = function (mode) {
			this.mode = mode;
		};
		this.doForgetPassword = function () {
			var form = $scope.landingForm;
			form.$setSubmitted();
			for (var key in form) {
				//console.log(form[key]);
				if (form.hasOwnProperty(key) && form[key] && form[key].$touched !== undefined) {
					form[key].$setTouched();
				}
			}
			if (form.$valid) {
				userService.forgetPassword(self.username).then((msg) => {
					toastr.success(msg, 'Success :)');
				}).catch((err) => {
					if(err.data && err.data.code){
                        $translate(`error.${err.data.code}`).then(msg => toastr.error(msg, 'Failure :('));
                    } else {
                        toastr.error('Failed to retrieve password!', 'Failure :(');
                    }
				});
			}
		}
	}

	angular.module("angularApp.routes").component("landing", {
		templateUrl: "routes/landing.html",
		bindings: {
			oneWay: '@',
			twoWay: '=',
			$router: '<'
		},
		$routeConfig: [],
		$routerCanReuse: true,
		controller: landingController
	});
})();