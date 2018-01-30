(function () {

    function createQuiz($scope, $log, $filter,educationService) {
        /********************************************************
         * Objects being used to create quiz
         ********************************************************/
        $scope.Quiz = {
            QuizID:"",
            Name: "",
            Quizversion: "",
            QuizDescription: "",
            RelatedSource: "",
            Scale: "",
            Status: "",
            LastModifiedDate: "",
            QuizQuestions: [],
        };

        $scope.QuizQuestion = {
            Question: "",
            QuestionID: "",
            Answers: [],
            CorrectA: ""
        };

        $scope.QuizAnswer = {
            Answer: "",
            AnswerID: "",
            QuestionID: ""

        };



        $scope.QuestionID = 1;
        $scope.showHelp = true;

            educationService.listDocuments().then(function(docs){
                $scope.documents = docs;
                
            });




        /**************************************************************
         This function adds new question field to the page
         **************************************************************/

        $scope.addQuestion = function () {
            $scope.QuizQuestion.Answers = [];
            $scope.QuizQuestion.CorrectA = "";
            $scope.QuizQuestion.Question = "";
            $scope.QuizQuestion.QuestionID = "";
            

            //Adding the Answer to answerList of the Question
            for (i = 0; i < 2; i++) {

                $scope.QuizAnswer.QuestionID = 0;
                $scope.QuizAnswer.AnswerID = 0;
                $scope.QuizAnswer.QuestionID =$scope.QuestionID;
                $scope.QuizAnswer.AnswerID = i + 1;
                $scope.QuizQuestion.Answers.push($scope.QuizAnswer);
                $scope.QuizAnswer = {};
            }

            $scope.QuizQuestion.QuestionID = $scope.QuestionID;
            $scope.Quiz.QuizQuestions.push($scope.QuizQuestion);
            $scope.QuizQuestion = {};
            $scope.showHelp = false;
            $scope.QuestionID+=1;

        }
        ;

        /**************************************************************
         This function adds new question field to the page
         **************************************************************/
        $scope.addAnswerfield = function (QID) {
        //gets the lenght of the lenght of the Answer list inside the question to find the AnswerID
        $scope.AnswerCounter = $scope.Quiz.QuizQuestions[QID-1].Answers.length;
        $scope.QuizAnswer.QuestionID = 0;
        $scope.QuizAnswer.AnswerID = 0;
        $scope.QuizAnswer.QuestionID = QID;
        $scope.QuizAnswer.AnswerID = $scope.AnswerCounter+1;
        $scope.Quiz.QuizQuestions[QID-1].Answers.push($scope.QuizAnswer);
        $scope.QuizAnswer = {};
        
  

        };

        /**************************************************************
         Delete Question or Answer
         **************************************************************/
        //delete Answer
        $scope.deleteAnswer = function (QuestionID, AID) {
            $log.log(AID);
            index = 0;
            i = 0;
            angular.forEach($scope.Quiz.QuizQuestions[QuestionID - 1].Answers, function (Aobj) {

                if (Aobj.AnswerID === AID) {
                    index = i;
                }

                i += 1;
            });


            $scope.Quiz.QuizQuestions[QuestionID - 1].Answers.splice(index, 1);
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


        /**************************************************************
         Creating the JSON String
         **************************************************************/

        $scope.submit = function (condition) {
            
            $scope.Quiz.QuizID = 0;
            $scope.Quiz.Quizversion = 1;
            $scope.Quiz.Scale = ($scope.Quiz.QuizQuestions.length * 10);
            $scope.currentDate = new Date();
            $scope.date = $filter('date')($scope.currentDate, "M/d/yy h:mm:ss a");
            $scope.Quiz.LastModifiedDate = $scope.date;
            $scope.Quiz.Status = condition;
            $scope.Quiz.Name = $scope.QuizName;
            $scope.Quiz.QuizDescription = $scope.Description;
            $scope.Quiz.RelatedSource = $scope.relatedTopic;

            $scope.String = JSON.stringify($scope.Quiz);
            $log.info($scope.String);
     
                $log.log($scope.String);
           educationService.insertQuiz($scope.String).then(function(obj){
                toastr.success('You have successfully added '+ $scope.Quiz.Name + 'quiz', 'success!!');  
                
           }).catch(() => {
				toastr.error('Failed to add the '+ $scope.Quiz.Name + 'quiz', 'Failure!!');      
			});
            
         

        }
        

    }
    


    angular.module("angularApp.routes").component('createQuizAdmin', {
        templateUrl: "routes/admin/education/createQuiz.html",
        controller: createQuiz

    });

})();

