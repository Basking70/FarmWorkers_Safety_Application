(function() {
    function videoEducationAdminController($scope, educationService, Video) {
        var self = this;
        this.videoList = [];
        
        self.$onInit = function(){
            self.initComponent();
        }
        self.initComponent = function(){
            self.newVideo = {
                TypeEducationalContent: "Video"
            };
        	educationService.listVideos(true).then(function(videos){
                self.videoList = videos;
            });
        };
        self.doAdd = function(){
            educationService.insertVideo(self.newVideo).then(function(message){
                self.initComponent();
            });
        };
        self.toggleUpdateVideo = function(video) {
            self.isUpdate = true;
            angular.copy(video,self.newVideo);
        }
        self.doUpdate = function(){
            educationService.updateVideo(self.newVideo).then((updatedVideo) => {
                var index = self.videoList.findIndex(x => x.IDEducationalContent === updatedVideo.IDEducationalContent);
                self.videoList[index] = updatedVideo;
                self.newVideo = new Video();
                self.isUpdate = false;
                toastr.success('Update successfully!', 'Success :)');
            });
            
        }
        self.doCancelUpdate = function(){
            self.isUpdate = false;
            self.newVideo = new Video();
        }
        self.doDisableVideo= function(video){
            var targetVideo = angular.copy(video);
            targetVideo.IsActive = 0;
            educationService.updateVideo(targetVideo).then((updatedVideo) => {
                var index = self.videoList.findIndex(x => x.IDEducationalContent === updatedVideo.IDEducationalContent);
                self.videoList[index] = updatedVideo;
                toastr.success('Disable successfully!', 'Success :)');
            });   
        }
        self.doEnableVideo = function(video){
            var targetVideo = angular.copy(video);
            targetVideo.IsActive = 1;
            educationService.updateVideo(targetVideo).then((updatedVideo) => {
                var index = self.videoList.findIndex(x => x.IDEducationalContent === updatedVideo.IDEducationalContent);
                self.videoList[index] = updatedVideo;
                toastr.success('Enable successfully!', 'Success :)');
            });   
        }
    }
    angular.module("angularApp.routes").component("videoEducationAdmin", {
        templateUrl: "routes/admin/education/video.html",
        controller: videoEducationAdminController
    });
})();