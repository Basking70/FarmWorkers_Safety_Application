(function() {
    function quizEducationAdminController($scope, educationService) {
        var self = this;
        this.quizList = [];
        this.newQuiz = {
            TypeEducationalContent: "Quiz"
        };
        self.$onInit = function(){
        	self.initComponent();
        };
        self.initComponent = function(){
            self.newQuiz = {
                TypeEducationalContent: "Quiz"
            };
        	educationService.listQuizzes().then(function(quizzes){
                self.quizList = quizzes;
            });
        };
        self.doAdd = function(){
            educationService.insertQuiz(self.newQuiz).then(function(message){
                self.initComponent();
            });
        };
        self.doEdit = function(quiz){
            console.log("Call edit quiz with ", quiz);
            this.$router.navigate(['AdminEditQuiz', {id: quiz.QuizID, version: quiz.QuizVersion}]);
        }
        
        self.dodelete = function(quiz){
            confirm('Are you sure you want to delete'+ quiz.QuizName);
            educationService.deleteQuiz(quiz.QuizID, quiz.QuizVersion).then(function(obj){
               toastr.success('You have succesfully deleted '+ quiz.QuizName+' quiz', 'Success!'); 
            }).catch(() => {
				toastr.error('Failed to delete '+ quiz.QuizName + 'quiz', 'Failure!!');      
			});
            
       }
        
    }
    angular.module("angularApp.routes").component("quizEducationAdmin", {
        templateUrl: "routes/admin/education/quiz.html",
        controller: quizEducationAdminController,
         bindings:{$router: '<'}
    });
})();