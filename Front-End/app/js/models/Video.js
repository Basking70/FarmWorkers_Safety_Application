(function(){
	function VideoContentFactory(){
		function youtube_parser(url){
		    var regExp = /^.*((youtu.be\/)|(v\/)|(\/u\/\w\/)|(embed\/)|(watch\?))\??v?=?([^#\&\?]*).*/;
		    var match = url.match(regExp);
		    return (match&&match[7].length==11)? match[7] : false;
		}

		function Video(obj = {}){
			var json = obj ? obj : {};
			this.IDEducationalContent = json.IDEducationalContent;
			this.LinkEducationalContent = json.LinkEducationalContent;
			this.TypeEducationalContent = json.TypeEducationalContent;
			this.DescriptionEducationalContent = json.DescriptionEducationalContent;
			this.DateEducationalContentCreated = json.DateEducationalContentCreated;
			this.IsActive = json.IsActive;
			this.YoutubeId = youtube_parser(this.LinkEducationalContent || '');
		}
		return Video;
	}
	angular.module("angularApp.models").factory("Video",VideoContentFactory);
})();