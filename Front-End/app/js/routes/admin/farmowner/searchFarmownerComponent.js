(function() {
    function searchFarmownerController($scope, farmownerService, NgTableParams, User) {
        var self = this;
        self.isSearched = false;
        self.tableParams = new NgTableParams();
        self.search = {};
        self.$routerCanReuse = function() {
            return false;
        }
        self.doSearch = function(){
            self.isSearched = true;
            farmownerService.searchFarmowners(self.search.email,self.search.phone,self.search.name).then((farmowners)=>{   
                self.farmowners = farmowners;
                self.refreshTable()
            });
        }
         self.refreshTable = function(){
            self.tableParams.settings({
                    dataset: self.farmowners
                });
        }
        self.toggleSearchResult = function(){
            self.isSearched = !self.isSearched;
        }
        self.toggleUpdateFarmOwner = function (user) {
            self.isUpdate = true;
            self.updateFarmOwner = angular.copy(user);
        }
        self.doUpdateFarmOwner = function () {
            farmownerService.updateFarmowner(self.updateFarmOwner).then((updatedUser) => {
                toastr.success('Update successfully!', 'Success :)');
                var index = self.farmowners.findIndex(x => x.UserID === updatedUser.UserID);
                self.farmowners[index] = updatedUser;
                self.refreshTable();
                self.updateFarmOwner = new User();
                self.isUpdate = false;
            });
        }
        self.doCancelUpdate = function () {
            self.updateFarmOwner = new User();
            self.isUpdate = false;
        }
    }
    angular.module("angularApp.routes").component("searchFarmowner", {
        templateUrl: "routes/admin/farmowner/search.html",
        controller: searchFarmownerController
    });
})();