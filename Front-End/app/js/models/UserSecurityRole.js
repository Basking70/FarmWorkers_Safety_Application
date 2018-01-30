(function(){
        function UserSecurityRoleFactory(){
                function UserSecurityRole(obj = {}){
                        var json = obj ? obj : {};
                        this.UserRoleID = json.UserRoleID;
                        this.UserSecurityRoleName = json.UserSecurityRoleName;
                        this.UserSecurityRoleDescription = json.UserSecurityRoleDescription;
                        this.IsActive = json.IsActive;
                }
                return UserSecurityRole;
        }
        angular.module("angularApp.models").factory("UserSecurityRole", UserSecurityRoleFactory);
})();