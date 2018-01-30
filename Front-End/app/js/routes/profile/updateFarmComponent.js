(function(){
	function updateFarmController(userService, Farm, UserFarm){
		var self = this;
		this.$routerOnActivate = function(next, previous) {
			console.log("inside");
            self.name = next.params.name;
            self.credential = userService.getCredentialInfo();
			
			// clone object for using only in this component
			self.currentUserFarms = self.credential.User.UserFarms;
			console.log(self.currentUserFarms);
			if(userService.getCurrentRole() === 'Farm Worker' && self.currentUserFarms.length === 0){
					self.addUserFarm();
			}
        };
		this.addUserFarm = function(){
			var newUserFarm = new UserFarm({
				UserID: self.credential.UserID,
			});
			self.currentUserFarms.push(newUserFarm);
			self.setLatest(newUserFarm);
		}
		this.removeUserFarm = function(userFarm){
			var removeIndex = self.currentUserFarms.indexOf(userFarm);
			self.currentUserFarms.splice(removeIndex, 1);
		}
		this.setLatest = function(userFarm){
			angular.forEach(self.currentUserFarms, function(x){
				x.IsLatest = 0;
			});
			userFarm.IsLatest = 1;
			console.log(self.currentUserFarms);
		}
		this.doUpdate = function(){
			var updatedUserFarm = self.currentUserFarms.map(x => ({
				UserID: self.credential.UserID,
				FarmID: x.FarmID,
				IsLatest: x.IsLatest
			}));
			userService.updateUserFarm(updatedUserFarm).then(function(userFarms){
				self.credential.User.UserFarms = userFarms.map(x=> new UserFarm(x));
				console.log(userFarms.find(x=>x.IsLatest==1));
				self.credential.User.CurrentFarm = new Farm(userFarms.find(x=>x.IsLatest==1).Farm);
                toastr.success('Update successfully!', 'Success :)');
			});
		};
		this.changeFarm = function(a){
			console.log(a);
		}
	}
	angular.module("angularApp.routes").component("updateFarm",{
		templateUrl: "routes/profile/farm.html",
		controller: updateFarmController
	});
})();