(function(){
	function UserCommunicationPreferenceFactory(QuizAnswer){
		function UserCommunicationPreference(obj = {}){
			var json = obj ? obj : {};
			this.IDUserCommunicationPreference = json.IDUserCommunicationPreference;
			this.UserCommunicationPreferenceFrequency = json.UserCommunicationPreferenceFrequency; // Monthly, Daily, Weekly, Never
			this.UserCommunicationPreferenceNotificationType = json.UserCommunicationPreferenceNotificationType; // Email, Phone, None
			this.IDUserCommunicationPreferenceLanguage = json.IDUserCommunicationPreferenceLanguage; // English, Spanish
			this.UserID = json.UserID;
		}
		return UserCommunicationPreference;
	}
	angular.module("angularApp.models").factory("UserCommunicationPreference",UserCommunicationPreferenceFactory);
})();