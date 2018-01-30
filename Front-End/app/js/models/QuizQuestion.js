(function(){
	function QuizQuestionFactory(QuizAnswer){
		function QuizQuestion(obj = {}){
			var json = obj ? obj : {};
			this.QuestionID = json.QuestionID;
			this.QuizID = json.QuizID;
			this.QuizVersion = json.QuizVersion;
            this.CorrectAnswer = json.CorrectAnswer;
			this.Question = json.Question;
			this.DateModified = json.DateModified;
			this.IsActive = json.IsActive;
			this.QuizAnswers = json.QuizAnswers ? json.QuizAnswers.map(function(answer){ 
				return new QuizAnswer(answer); }) : [];
		}
		return QuizQuestion;
	}
	angular.module("angularApp.models").factory("QuizQuestion",QuizQuestionFactory);
})();