(function(){
	function LoginCredentialFactory(User, UserSecurityRole){
		function LoginCredential(obj = {}){
			var json = obj ? obj : {};
			this.UserLoginID = json.UserLoginID;
			this.User = new User(json.User);
			this.UserID = this.User.UserID;
			this.UserSecurityRole = new UserSecurityRole(json.UserSecurityRole);
			this.UserRoleID = this.UserSecurityRole.UserRoleID;
			this.Password = json.Password;
			this.IsActive = json.IsActive;
		}
		return LoginCredential;
	}
	angular.module("angularApp.models").factory("LoginCredential",LoginCredentialFactory);
})();