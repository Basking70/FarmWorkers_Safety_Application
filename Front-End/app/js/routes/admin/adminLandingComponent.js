(function() {
    function adminLandingController($scope, educationService, farmService, userService) {
        var self = this;
        self.farmworkerCount = 0;
        self.farmCount = 0;
        self.videoCount = 0;
        self.quizCount = 0;
        self.documentCount = 0;
        self.$routerOnActivate = function(){
            educationService.listQuizzes().then(quizzes => {
                self.quizCount = quizzes.length;
            });
            educationService.listVideos().then(videos => {
                self.videoCount = videos.length;
            });
            educationService.listDocuments().then(docs => {
                self.documentCount = docs.length;
            });
            farmService.listFarms().then(farms => {
                self.farmCount = farms.length;
            });
            userService.listUsersByUserType("Farm Worker").then(farmworkers => {
                self.farmworkerCount = farmworkers.length;
            });
        }
    }
    angular.module("angularApp.routes").component("adminLanding", {
        templateUrl: "routes/admin/landing.html",
        controller: adminLandingController
    });
})();