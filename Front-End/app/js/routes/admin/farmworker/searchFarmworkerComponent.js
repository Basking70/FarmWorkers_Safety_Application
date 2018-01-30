(function () {
    function searcFarmworkerController($scope, farmworkerService, NgTableParams, userService, User) {
        var self = this;
        self.isSearched = false;
        self.tableParams = new NgTableParams();
        self.search = {};
        self.$routerCanReuse = function () {
            return false;
        }
        self.doSearch = function () {
            self.isSearched = true;
            farmworkerService.searchFarmworkers(self.search.email, self.search.phone, self.search.name).then((farmworkers) => {
                self.farmworkers = farmworkers;
                self.refreshTable();
            });
        }
        self.refreshTable = function () {
            self.tableParams.settings({
                dataset: self.farmworkers
            });
        }
        self.toggleSearchResult = function () {
            self.isSearched = !self.isSearched;
        }
        self.toggleUpdateFarmWorker = function (user) {
            self.isUpdate = true;
            self.updateFarmWorker = angular.copy(user);
        }
        self.doUpdateFarmWorker = function () {
            farmworkerService.updateFarmworker(self.updateFarmWorker).then((updatedUser) => {
                toastr.success('Update successfully!', 'Success :)');
                var index = self.farmworkers.findIndex(x => x.UserID === updatedUser.UserID);
                self.farmworkers[index] = updatedUser;
                self.refreshTable();
                self.updateFarmWorker = new User();
                self.isUpdate = false;
            });
        }
        self.doCancelUpdate = function () {
            self.updateFarmWorker = new User();
            self.isUpdate = false;
        }
    }
    angular.module("angularApp.routes").component("searchFarmworker", {
        templateUrl: "routes/admin/farmworker/search.html",
        controller: searcFarmworkerController
    });
})();