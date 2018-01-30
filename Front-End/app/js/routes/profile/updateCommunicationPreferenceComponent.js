(function(){
	function updateCommunicationPreferenceController(userService, UserCommunicationPreference){
		var self = this;
        self.notificationTypes = [ 'Email', 'Phone'];
        self.frequencies = ['Monthly', 'Weekly', 'Daily', 'Never'];
        self.languages = ['English', 'Spanish'];
        self.currentPreferences = [];
		self.$routerOnActivate = function(next, previous) {
            self.name = next.params.name;
            self.credential = userService.getCredentialInfo();
			self.currentUserCommunicationPreferences = self.credential.User.UserCommunicationPreferences;
            angular.forEach(self.notificationTypes, (notificationType) => {
                var preference = self.credential.User.UserCommunicationPreferences.find(x => x.UserCommunicationPreferenceNotificationType === notificationType);
                if(preference) {
                    self.currentPreferences.push(preference);
                } else {
                    self.currentPreferences.push(
                        new UserCommunicationPreference({
                            UserCommunicationPreferenceNotificationType: notificationType,
                            IDUserCommunicationPreferenceLanguage: 'English',
                            UserCommunicationPreferenceFrequency: 'Never',
                            UserID: self.credential.UserID
                        })
                    );
                }
            }); 
        };
        self.doUpdate = function(){
            console.log(self.currentPreferences);
            userService.updateUserCommunicationPreference(self.currentPreferences).then((preferences) => {
                self.credential.User.UserCommunicationPreferences = preferences;
                toastr.success('Update successfully!', 'Success :)');
            });
        }
	}
	angular.module("angularApp.routes").component("updateCommunicationPreference",{
		templateUrl: "routes/profile/communicationPreference.html",
		controller: updateCommunicationPreferenceController
	});
})();