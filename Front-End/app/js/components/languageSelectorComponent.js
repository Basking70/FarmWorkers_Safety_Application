(function(){
	function languageSelectorComponentController($translate){
		this.getCurrentLanguage = function(){
			return $translate.use();
		}
		this.changeLanguage = function (key) {
		    $translate.use(key);
		};
	}
	angular.module("angularApp.components").component("languageSelector",{
		templateUrl: "components/language-selector.html",
		controller: languageSelectorComponentController,
		bindings: {
			showLabel:'@'
		}
	});
})();