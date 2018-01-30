(function() {
    function documentEducationAdminController($scope, educationService, Document) {
        var self = this;
        this.documentList = [];
        self.isUpdate = false;
        self.$onInit = function(){
        	self.initComponent();
        };
        self.initComponent = function(){
            self.newDocument = {
                TypeEducationalContent: "Document"
            };
            educationService.listDocuments(true).then(function(documents){
                self.documentList = documents;
            });
        }
        self.doAdd = function(){
            educationService.insertDocument(self.newDocument).then(function(message){
                self.initComponent();
            });
        };
        self.toggleUpdateDocument = function(document) {
            self.isUpdate = true;
            self.newDocument = angular.copy(document);
        }
        self.doUpdate = function(){
            educationService.updateDocument(self.newDocument).then((updatedDocument) => {
                var index = self.documentList.findIndex(x => x.IDEducationalContent === updatedDocument.IDEducationalContent);
                self.documentList[index] = updatedDocument;
                self.newDocument = new Document();
                self.isUpdate = false;
                toastr.success('Update successfully!', 'Success :)');
            });
            
        }
        self.doDisableDocument = function(document){
            var toBeDeleteDocument = angular.copy(document);
            toBeDeleteDocument.IsActive = 0;
            educationService.updateDocument(toBeDeleteDocument).then((updatedDocument) => {
                var index = self.documentList.findIndex(x => x.IDEducationalContent === updatedDocument.IDEducationalContent);
                self.documentList[index] = updatedDocument;
                toastr.success('Enable successfully!', 'Success :)');
            });   
        }
        self.doEnableDocument = function(document){
            var toBeDeleteDocument = angular.copy(document);
            toBeDeleteDocument.IsActive = 1;
            educationService.updateDocument(toBeDeleteDocument).then((updatedDocument) => {
                var index = self.documentList.findIndex(x => x.IDEducationalContent === updatedDocument.IDEducationalContent);
                self.documentList[index] = updatedDocument;
                toastr.success('Disable successfully!', 'Success :)');
            });   
        }
        self.doCancelUpdate = function(){
            self.isUpdate = false;
            self.newDocument = new Document();
        }
        
    }
    angular.module("angularApp.routes").component("documentEducationAdmin", {
        templateUrl: "routes/admin/education/document.html",
        controller: documentEducationAdminController
    });
})();