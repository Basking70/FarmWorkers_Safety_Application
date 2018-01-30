(function(){
	function QuizFactory(QuizQuestion, Document){
		function Quiz(obj = {}){
			var json = obj ? obj : {};         
			this.QuizID  = json.QuizID ;
			this.QuizVersion  = json.QuizVersion;
			this.QuizName   = json.QuizName;
			this.QuizDescription = json.QuizDescription;
			this.Scale = json.Scale;
			this.DateModified = json.DateModified;
			this.IsActive = json.IsActive;
			this.QuizQuestions = json.QuizQuestions ? json.QuizQuestions.map(function(question){ 
				return new QuizQuestion(question); }) : [];
            this.EducationalContents = json.EducationalContents ? json.EducationalContents.map(function(docs){ 
				return new Document(docs); }) : [];
		}
		return Quiz;
	}
	angular.module("angularApp.models").factory("Quiz",QuizFactory);
})();