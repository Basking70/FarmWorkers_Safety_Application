(function(){
        function UserFactory(UserFarm, Farm, UserCommunicationPreference){
                function User(obj = {}){
                        var json = obj ? obj : {};
                        this.UserID = json.UserID;
                        this.UserType = json.UserType;
                        this.UserName = json.UserName;
                        this.UserLastName = json.UserLastName;
                        this.UserDateOfBirth = json.UserDateOfBirth;
                        this.UserHeight = json.UserHeight;
                        this.UserWeight = json.UserWeight;
                        this.UserGender = json.UserGender;
                        this.UserEmail = json.UserEmail;
                        this.UserPhoneNumber = json.UserPhoneNumber;
                        this.UserWorkLocation = json.UserWorkLocation;
                        this.UserMemberSince = json.UserMemberSince;
                        this.UserFarmID = json.UserFarmID;
                        this.IsActive = json.IsActive;
                        this.UserFarms = json.UserFarms ? json.UserFarms.map(x => new UserFarm(x)) : [];
                        this.UserCommunicationPreferences = json.UserCommunicationPreferences ? json.UserCommunicationPreferences.map(x => new UserCommunicationPreference(x)) : [];
                        this.CurrentFarm = new Farm(json.CurrentFarm);
                        // Object.defineProperty(this, 'CurrentFarm', { 
                        //         get: () => { 
                        //                 var latestFarm = this.UserFarms.find((x) => x.IsLatest === 1);
                        //                 return latestFarm ? latestFarm.Farm : undefined;
                        //         } 
                        // });
                }
                return User;
        }
        angular.module("angularApp.models").factory("User", UserFactory);
})();