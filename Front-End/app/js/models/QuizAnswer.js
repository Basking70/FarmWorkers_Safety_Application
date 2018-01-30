(function(){
	function QuizAnswerFactory(){
		function QuizAnswer(obj = {}){
			var json = obj ? obj : {};
			this.AnswerID = json.AnswerID;
			this.QuestionID = json.QuestionID;
			this.QuizID = json.QuizID;
			this.QuizVersion = json.QuizVersion;
			this.Answer = json.Answer;
			this.DateModified = json.DateModified;
			this.IsActive = json.IsActive;
			this.IsCorrect = json.IsCorrect;
			
		}
		return QuizAnswer;
	}
	angular.module("angularApp.models").factory("QuizAnswer",QuizAnswerFactory);
})();