(function(){
	function EducationContentMappingFactory(){
		function EducationContentMapping(obj = {}){
			var json = obj ? obj : {};
			this.EducationalContentID = json.EducationalContentID;
			this.UserID = json.UserID;
			this.UserEducationalContentClicked = json.UserEducationalContentClicked;
			this.UserEducationalContentDateCreated = json.UserEducationalContentDateCreated;
		}
		return EducationContentMapping;
	}
	angular.module("angularApp.models").factory("EducationContentMapping",EducationContentMappingFactory);
})();