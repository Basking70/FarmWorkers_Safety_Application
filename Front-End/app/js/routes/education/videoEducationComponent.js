(function() {
    function videoEducationController($scope, userService, educationService, videoModalService) {
        var self = this;
        this.videoList = [];
        this.displayedVideoList = [];
        this.videoMappings = null;
        this.searchKeyword = "";
        self.$onInit = function(){
            self.currentUser = userService.getUserInfo()
        	educationService.listVideos().then(function(videos){
                self.videoList = videos;
                self.displayedVideoList = self.videoList;    
            });
            educationService.listUserVideoMappings(self.currentUser.UserID).then(function(videoMappings){
                self.videoMappings = videoMappings;
            });
        }
        self.checkIsWatched = function(videoId){
            if(!self.videoMappings) return false;
            return !!self.videoMappings.find(x => x.EducationalContentID === videoId && x.UserEducationalContentClicked === "1");
        }
        self.openVideo = function(video){
            educationService.mapContentToUser(self.currentUser, video).then((mappingInfo) => {
                var existedIndex = self.videoMappings.findIndex(x => x.EducationalContentID === video.IDEducationalContent);
                if(existedIndex >= 0) {
                    self.videoMappings[existedIndex] = mappingInfo;
                } else {
                    self.videoMappings.push(mappingInfo);
                }
            });
            videoModalService.createModal(video);
        }
        $scope.$watch(
        	function() { return self.searchKeyword; },
        	function(keyword){
	        	self.displayedVideoList = self.videoList.filter(function(video){
	        		return video.DescriptionEducationalContent.toLowerCase().indexOf(keyword.toLowerCase())>=0;
	        	});
        	}
        );
    }
    angular.module("angularApp.routes").component("videoEducation", {
        templateUrl: "routes/education/video.html",
        controller: videoEducationController
    });
})();