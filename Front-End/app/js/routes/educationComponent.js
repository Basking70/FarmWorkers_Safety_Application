(function() {
    function educationModuleController($scope) {
        var self = this;
        self.isActive = function(childRouteName) {
		    var instruction = self.$router.generate([childRouteName]);
		    return self.$router.isRouteActive(instruction);
	  	}
    }
    angular.module("angularApp.routes").component("education", {
        templateUrl: "routes/education.html",
        controller: educationModuleController,
        $routeConfig : [
        {
            path: '/video',
            component: 'videoEducation',
            name: 'VideoEducation',
            useAsDefault: true
        },{
            path: '/quiz',
            component: 'quizEducation',
            name: 'QuizEducation'
        },
        {
            path: '/document',
            component: 'documentEducation',
            name: 'DocumentEducation'
        },
        {
            path: '/takequiz',
            component: 'takeQuiz',
            name: 'UserTakeQuiz'
        }

        ],
        bindings: {
			$router : '<'
		}
    });
})();