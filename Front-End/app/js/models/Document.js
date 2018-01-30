(function(){
	function DocumentFactory(){
		function Document(obj = {}){
			var json = obj ? obj : {};          
			this.IDEducationalContent = json.IDEducationalContent;
			this.LinkEducationalContent = json.LinkEducationalContent;
			this.TypeEducationalContent = json.TypeEducationalContent;
			this.DescriptionEducationalContent = json.DescriptionEducationalContent;
			this.DateEducationalContentCreated = json.DateEducationalContentCreated;
			this.IsActive = json.IsActive;
		}
		return Document;
	}
	angular.module("angularApp.models").factory("Document",DocumentFactory);
})();