(function() {
    function educationAdminController($scope, $q) {
        var self = this;
        self.isActive = function(childRouteName) {
		    var instruction = self.$router.generate(childRouteName);
		    return self.$router.isRouteActive(instruction);
	  	}
      
    }
    angular.module("angularApp.routes").component("educationAdmin", {
        templateUrl: "routes/admin/education.html",
        controller: educationAdminController,
        $routeConfig : [
		{
            path: '/document',
            component: 'documentEducationAdmin',
            name: 'DocumentEducationAdmin',
            useAsDefault: true
        },
        {
            path: '/video',
            component: 'videoEducationAdmin',
            name: 'VideoEducationAdmin'
        },{
            path: '/quiz',
            component: 'quizEducationAdmin',
            name: 'QuizEducationAdmin'
        },
        {
            path: '/editquiz',
            component: 'editQuizAdmin',
            name: 'AdminEditQuiz'
        },
        {
            path: '/createquiz',
            component: 'createQuizAdmin',
            name: 'AdminCreateQuiz'
        }
        ],
        bindings: {
			$router : '<'
		}
    });
})();