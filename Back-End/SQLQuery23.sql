SELECT QQ.QuestionID, QQ.Question, QQ.QuizVersion, QQ.QuizID, QQ.IsActive, QQ.DateModified,
		(SELECT QA.AnswerID 
		 FROM QuizAnswers QA 
		 WHERE QA.QuizID = QQ.QuizID AND QA.QuizVersion = QQ.QuizVersion AND QA.QuestionID = QQ.QuestionID AND QA.IsCorrect = '1') AS CorrectAnswer
FROM QuizQuestions as QQ
WHERE QQ.QuizID = 1 AND
	  QQ.QuizVersion = 1 AND
	  QQ.IsActive = '1'