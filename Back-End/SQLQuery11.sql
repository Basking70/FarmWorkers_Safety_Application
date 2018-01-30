use FarmWorkersAppDatabaseV4
SELECT Q.QuizID, Q.QuizVersion, Q.QuizName, Q.QuizDescription, Q.Scale, Q.IsActive, Q.DateModified,
	   QQ.QuestionID, QQ.Question, QQ.IsActive, QQ.DateModified,
	   QA.AnswerID, QA.Answer, QA.IsCorrect, QA.IsActive, QA.DateModified
FROM Quiz as Q, QuizQuestions as QQ, QuizAnswers as QA
WHERE Q.QuizID = QQ.QuizID AND
	  QQ.QuizID = QA.QuizID AND
	  Q.QuizVersion = QQ.QuizVersion AND
	  QQ.QuizVersion = QA.QuizVersion AND
	  QQ.QuestionID = QA.QuestionID
