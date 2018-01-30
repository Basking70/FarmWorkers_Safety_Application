(function() {
    function documentEducationController($scope, userService, educationService, Document) {
        var self = this;
        this.documentList = [];
        this.displayedDocumentList = [];
        this.documentMappings = null;
        this.searchKeyword = "";
        self.isUpdate = false;

        self.$onInit = function(){
            self.currentUser = userService.getUserInfo();
            educationService.listDocuments().then(function(documents){
                self.documentList = documents;
                self.displayedDocumentList = self.documentList;    
            });
            educationService.listUserDocumentMappings(self.currentUser.UserID).then(function(documentMappings){
                self.documentMappings = documentMappings;
            });
        }
        
        self.checkIsViewed = function(documentId){
            if(!self.documentMappings) return false;
            return !!self.documentMappings.find(x => x.EducationalContentID === documentId && x.UserEducationalContentClicked === "1");
        }

        self.openDocument = function(document){
            educationService.mapContentToUser(self.currentUser, document).then((mappingInfo) => {
                var existedIndex = self.documentMappings.findIndex(x => x.EducationalContentID === document.IDEducationalContent);
                if(existedIndex >= 0) {
                    self.documentMappings[existedIndex] = mappingInfo;
                } else {
                    self.documentMappings.push(mappingInfo);
                }
            });
        }
        $scope.$watch(
            function() { return self.searchKeyword; },
            function(keyword){
        	   self.displayedDocumentList = self.documentList.filter(function(document){
        	       return document.DescriptionEducationalContent.toLowerCase().indexOf(keyword.toLowerCase())>=0;
        	   });
            }
        );

    }
    angular.module("angularApp.routes").component("documentEducation", {
        templateUrl: "routes/education/document.html",
        controller: documentEducationController
    });
})();