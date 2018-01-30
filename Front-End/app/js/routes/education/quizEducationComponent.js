(function() {
    function quizEducationController($scope, userService, educationService) {
        var self = this;
        this.quizList = [];
        this.displayedQuizList = [];
        this.searchKeyword = "";
        self.$onInit = function(){
        	educationService.listQuizzes().then(function(quizzes){
                self.quizList = quizzes;
                self.displayedQuizList = self.quizList;    
            });
	        
        }
        self.takeQuiz = function(quiz) {
            console.log("do the redirect",quiz);
            this.$router.navigate(['UserTakeQuiz', {id: quiz.QuizID, version: quiz.QuizVersion}]);
        }
        $scope.$watch(
            function() { return self.searchKeyword; },
            function(keyword){
            	self.displayedQuizList = self.quizList.filter(function(quiz){
            		return quiz.Name.indexOf(keyword)>=0;
            	});
            }
        );
    }
    angular.module("angularApp.routes").component("quizEducation", {
        templateUrl: "routes/education/quiz.html",
        controller: quizEducationController,
        bindings:{$router: '<'}
    });
    
    
})();