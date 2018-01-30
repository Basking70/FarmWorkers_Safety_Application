(function () {
    function QuizContent($scope, $log, $filter, educationService, userService) {



        this.$routerOnActivate = function (next) {
            
            $scope.gradeVisible = true;
            $scope.submited = false;
            $scope.GradeMessage = "";
            $scope.gradeClass;
            $scope.Score = 0;
            
            $scope.currentUser = userService.getUserInfo();
            $scope.currentDate = new Date();
            $scope.date = $filter('date')($scope.currentDate, "M/d/yy h:mm:ss a");

            $scope.id = next.params.id;
            $scope.version = next.params.version;
            educationService.getQuizByID($scope.id, $scope.version).then(function (quiz) {
                
                $scope.Quiz = quiz;
                $scope.Educational = $scope.Quiz.EducationalContents[0];
                    //{"QuizID":0,"Name":"Testing quiz","Quizversion":1,"QuizDescription":"Testin thr quiz taking","Scale":20,"Status":1,"LastModifiedDate":"4/11/17 5:27:15 PM","QuizQuestions":[{"Question":"Question #1","QuestionID":1,"Answers":[{"Answer":"Answer#1","AnswerID":1,"QuestionID":1,"$$hashKey":"object:50"},{"QuestionID":1,"AnswerID":2,"$$hashKey":"object:51","Answer":"Answer#2"},{"QuestionID":1,"AnswerID":3,"$$hashKey":"object:56","Answer":"This is the answer"}],"CorrectAnswer":"3","$$hashKey":"object:48"},{"Answers":[{"QuestionID":2,"AnswerID":1,"$$hashKey":"object:62","Answer":"This is the answer"},{"QuestionID":2,"AnswerID":2,"$$hashKey":"object:63","Answer":"Answer#2"}],"CorrectAnswer":"1","Question":"Question #2","QuestionID":2,"$$hashKey":"object:60"}]};

                //add selected 
                angular.forEach($scope.Quiz.QuizQuestions, function (obj) {
                    obj.Selected = 0;

                });

                
                
                
                
                /************************************
                 New object with the taken quiz data
                 ************************************/
                $scope.QuizResault = {
                    UserID: $scope.currentUser.UserID,
                    QuizID: $scope.Quiz.QuizName,
                    QuizVersion: $scope.Quiz.QuizVersion,
                    UserAnswers: [],
                    IsFinished:"",
                    QuizScore: "",
                    DateTaken: $scope.date
                };
            });



            



        };






        /**********************************************************
         The function that generates a grade based on user response
         **********************************************************/
        $scope.grader = function () {
            $scope.submited = true;
            $scope.Score = 0;
            angular.forEach($scope.Quiz.QuizQuestions, function (Qobj) {

                $scope.QuizResault.UserAnswers.push(Qobj.Selected); //pushes the selected answers into a QuizResault object
                if (Qobj.Selected === Qobj.CorrectAnswer) {
                    $scope.Score += 10;
                    Qobj.Correct = 'alert-success';
                } else {
                    Qobj.Correct = 'alert-danger';
                }
            });
            $scope.QuizResault.QuizScore = $scope.Score; //store the Score into the QuizResault object
            $scope.QuizResault.IsFinished = "1";

            //Generates Message to display to the user after taking the quiz
            if ($scope.Score === $scope.Quiz.Scale) {
                $scope.GradeMessage = "Excellent work!";
                $scope.gradeClass = 'alert alert-success';
            } else if ($scope.Score === 0) {
                $scope.GradeMessage = "You werent able to answer any of the questions right. Please review the related material and retake the quiz.";
                $scope.gradeClass = 'alert alert-danger';
            } else {
                $scope.GradeMessage = "You need to work more on your skills please review the related documents again and re-take the quiz.";
                $scope.gradeClass = 'alert alert-warning';
            }
            
            $scope.gradeVisible = false;
            
            $scope.String = JSON.stringify($scope.QuizResault);
            $log.info($scope.String);
            
            educationService.InsertUserQuizAttempt($scope.String).then(function(obj){
                $log.log("Quiz attempt was saved");
                
           }).catch(() => {
				$log.log("Quiz attempt was not saved");      
			});

        };
    }

    angular.module("angularApp.routes").component('takeQuiz', {
        templateUrl: "routes/education/TakeQuiz.html",
        controller: QuizContent

    });

})();