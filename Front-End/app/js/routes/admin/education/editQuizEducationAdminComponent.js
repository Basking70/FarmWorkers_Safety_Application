(function () {
    function EditQuizContent($scope, $log, $filter, educationService) {

        this.$routerOnActivate = function (next) {

            $scope.id = next.params.id;
            $log.log($scope.id);
            $scope.version = next.params.version;
            $scope.currentDate = new Date();
            $scope.date = $filter('date')($scope.currentDate, "M/d/yy h:mm:ss a");
            educationService.getQuizByID($scope.id, $scope.version).then(function (quiz) {
                $log.log("This is the quiz", quiz);
                $scope.Quiz = quiz;
            });


            $scope.QuizQuestion = {
                DateModified: "",
                IsActive: "",
                Question: "",
                QuestionID: "",
                QuizAnswers: [],
                CorrectAnswer: "",
                QuizID: "",
                QuizVersion: ""
            };

            $scope.QuizAnswer = {
                Answer: "",
                AnswerID: "",
                DateModified: "",
                IsActive: "",
                IsCorrect:"",
                QuestionID: "",
                QuizID: "",
                QuizVersion: ""
            };




        };




        /**************************************************************
         This function adds new question field to the page
         **************************************************************/

        $scope.addQuestion = function () {
            $scope.NumberOfQuestions = $scope.Quiz.QuizQuestions.length;

            $scope.QuizQuestion.QuizAnswers = [];
            $scope.QuizQuestion.CorrectAnswer = "";
            $scope.QuizQuestion.Question = "";
            $scope.QuizQuestion.QuestionID = "";

            //Adding the Answer to answerList of the Question
            for (i = 0; i < 2; i++) {

                $scope.QuizAnswer.QuestionID = 0;
                $scope.QuizAnswer.IsCorrect ="";
                $scope.QuizAnswer.AnswerID = 0;
                $scope.QuizAnswer.QuestionID = $scope.NumberOfQuestions + 1;
                $scope.QuizAnswer.QuizID = $scope.Quiz.QuizID;
                $scope.QuizAnswer.DateModified = $scope.date;
                $scope.QuizAnswer.IsActive = $scope.Quiz.IsActive;
                $scope.QuizAnswer.QuizVersion = $scope.Quiz.QuizVersion;
                $scope.QuizAnswer.AnswerID = i + 1;
                $scope.QuizQuestion.QuizAnswers.push($scope.QuizAnswer);
                $scope.QuizAnswer = {};
            }
            
            $scope.QuizQuestion.DateModified = $scope.date;
            $scope.QuizQuestion.QuizID = $scope.Quiz.QuizID;
            $scope.QuizQuestion.IsActive = $scope.Quiz.IsActive;
            $scope.QuizQuestion.QuizVersion = $scope.Quiz.QuizVersion;
            $scope.QuizQuestion.QuestionID = $scope.NumberOfQuestions + 1;
            $scope.Quiz.QuizQuestions.push($scope.QuizQuestion);
            $scope.QuizQuestion = {};
        };



        /**************************************************************
         This function adds new Answer field to the page
         **************************************************************/
        $scope.addAnswerfield = function (QID) {
            $scope.AnswerCounter = $scope.Quiz.QuizQuestions[QID - 1].QuizAnswers.length;
            $scope.QuizAnswer.IsCorrect ="";
            $scope.QuizAnswer.QuestionID = 0;
            $scope.QuizAnswer.AnswerID = 0;
            $scope.QuizAnswer.QuestionID = QID;
            $scope.QuizAnswer.QuizVersion = $scope.Quiz.QuizVersion;
            $scope.QuizAnswer.QuizID = $scope.Quiz.QuizID;
            $scope.QuizAnswer.DateModified = $scope.date;
            $scope.QuizAnswer.IsActive = $scope.Quiz.IsActive;
            $scope.QuizAnswer.AnswerID = $scope.AnswerCounter + 1;
            $scope.Quiz.QuizQuestions[QID - 1].QuizAnswers.push($scope.QuizAnswer);
            $scope.QuizAnswer = {};

        };


        /**************************************************************
         This function generates the JSON string
         **************************************************************/

        $scope.Submitted = function (status) {
            //$scope.Quiz.QuizVersion += 1; //Update the version

            
            $scope.Quiz.DateModified = $scope.date; //Update the date 
            //$scope.Quiz.RelatedSource = $scope.Quiz.EducationalContents[0].IDEducationalContent;
            $scope.Quiz.IsActive = status;
            //iterate through the questions to modify the satus, date and is correct fields
            angular.forEach($scope.Quiz.QuizQuestions, function(Question){
                    var correctAnswerID = Question.CorrectAnswer;
                    Question.DateModified = $scope.date;
                    Question.IsActive = status;
                    angular.forEach(Question.QuizAnswers, function(answer){
                        answer.DateModified = $scope.date;
                        answer.IsActive = status;
                        if(answer.AnswerID == correctAnswerID){
                            answer.IsCorrect = "1";
                            $log.log(answer.AnswerID +" : "+correctAnswerID);
                        }else{
                           answer.IsCorrect = "0"; 
                        }
                    });
            });
            $scope.String = JSON.stringify($scope.Quiz);
            $log.log($scope.String);
           educationService.updateQuiz($scope.String).then(function(obj){
                toastr.success('You have successfully added '+ $scope.Quiz.QuizName + 'quiz', 'success!!');  
                
           }).catch(() => {
				toastr.error('Failed to add the '+ $scope.Quiz.QuizName + 'quiz', 'Failure!!');      
			});
            
            
        };

        /**************************************************************
         Delete Question or Answer
         **************************************************************/
        //delete Answer
        $scope.deleteAnswer = function (QuestionID, AID) {
            $log.log(AID);
            index = 0;
            i = 0;
            angular.forEach($scope.Quiz.QuizQuestions[QuestionID - 1].QuizAnswers, function (Aobj) {

                if (Aobj.AnswerID === AID) {
                    index = i;
                }

                i += 1;
            });


            $scope.Quiz.QuizQuestions[QuestionID - 1].QuizAnswers.splice(index, 1);
        }
        //Delete Question
        $scope.deleteQuestion = function (QuestionID) {
            index = 0;
            i = 0;
            angular.forEach($scope.Quiz.QuizQuestions, function (Aobj) {

                if (Aobj.QuestionID === QuestionID) {
                    index = i;
                }
                i += 1;
            });

            $scope.Quiz.QuizQuestions.splice(index, 1);
        }

    }

    angular.module("angularApp.routes").component('editQuizAdmin', {
        templateUrl: "routes/admin/education/editQuiz.html",
        controller: EditQuizContent

    });

})();
