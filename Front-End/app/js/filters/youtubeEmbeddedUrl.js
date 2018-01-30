(function(){
	function youtubeEmbeddedUrl($sce) {
	    return function(videoId) {
	      return $sce.trustAsResourceUrl('http://www.youtube.com/embed/' + videoId);
	    };
  	}
	angular.module("angularApp.filters").filter('youtubeEmbeddedUrl',youtubeEmbeddedUrl);
})();