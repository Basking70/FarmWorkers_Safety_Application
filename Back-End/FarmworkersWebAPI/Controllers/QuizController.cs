using FarmworkersWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.Entity;

namespace FarmworkersWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class QuizController : ApiController
    {

        //***Entity Framework**//

        //NOTE: Watch out for conflicts, between: 
        //**FarmworkersWebAPI.Entities
        //**FarmworkersWebAPI.Models

            FarmworkersWebAPI.Entities.FarmWorkerAppContext _context;

            public QuizController()
            {
                _context = new FarmworkersWebAPI.Entities.FarmWorkerAppContext();
            }

            //Quiz
            [HttpPost]
            public IHttpActionResult EFInsertQuiz(ViewModels.QuizForm _quizInformation)
            {
                _context.Configuration.ProxyCreationEnabled = false;

                Entities.Quiz _quizReceived = new Entities.Quiz();
                Entities.Quiz _quiz = new Entities.Quiz();


                try
                {
                    //The incoming format coming from the Front-End is changed so it is adjusted to the Format in the data base.
                    _quizReceived = EFhandleIncomingQuizFormat(_quizInformation);

                    _quiz = _context.Quizs.Add(_quizReceived);
                    _context.SaveChanges();

                if (_quizInformation.RelatedSource != null)
                {
                    Entities.QuizEducationalContent _educationalContentLinkToQuiz = new Entities.QuizEducationalContent();

                    _educationalContentLinkToQuiz.QuizID = _quiz.QuizID;
                    _educationalContentLinkToQuiz.QuizVersion = _quiz.QuizVersion;
                    _educationalContentLinkToQuiz.EducationalContentID = int.Parse(_quizInformation.RelatedSource);

                    int _educationalContentID = int.Parse(_quizInformation.RelatedSource);

                    _context.QuizEducationalContents.Add(_educationalContentLinkToQuiz);

                    _context.SaveChanges();

                    _quiz.EducationalContents = _context.EducationalContents.Where(ec => ec.IDEducationalContent.Equals(_educationalContentID)).ToList();
                }                   

                    return Ok(_quiz);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }

            }

            [HttpPost]
            public IHttpActionResult EFupdateQuiz(ViewModels.QuizForm _quizInformation )
            {
                //The incoming format coming from the Front-End is changed so it is adjusted to the Format in the data base.
                Entities.Quiz _updatedQuiz = EFhandleIncomingQuizFormat(_quizInformation);

                if (_updatedQuiz.QuizID < 0)
                    return Ok("Quiz ID can not be negative");

                if (_updatedQuiz.QuizID == 0)
                    return Ok("Quiz ID Not Specified");

                _context.Configuration.ProxyCreationEnabled = false;

                try
                {
                    FarmworkersWebAPI.Entities.Quiz original = _context.Quizs.
                                                            Where(q => q.QuizID.Equals(_updatedQuiz.QuizID) && q.QuizVersion.Equals(_updatedQuiz.QuizVersion)).FirstOrDefault();

                    //***NOT Entity Framework
                    Quiz _updated = new Quiz();
                    ///***********************


                    if (original != null)
                    {

                        //Update Quiz Base Information
                        _context.Entry(original).CurrentValues.SetValues(_updatedQuiz);
                        _context.SaveChanges();


                        //Determine if there are less questions in this quiz update than in the previous version
                        //If there are less questions in the new update, the ones left over from the previous version 
                        //will be deleted

                        string _deletionResult = deleteOldLeftOverQuestions(_updatedQuiz);

                        if (_deletionResult != "Questions Deleted Successfully")
                        {
                            return Ok(_deletionResult);
                        }

                        foreach (FarmworkersWebAPI.Entities.QuizQuestion _question in _updatedQuiz.QuizQuestions)
                        {

                            string _updateResult = EFupdateQuizQuestion(_question);

                            if (_updateResult != "Question Updated Successfully")
                            {
                                return Ok(_updateResult);
                            }

                        }

                    _context.SaveChanges();

                    if (_quizInformation.RelatedSource != null)
                    {

                        Entities.QuizEducationalContent _updatedEducationalContentLinkToQuiz = new Entities.QuizEducationalContent();
                        Entities.QuizEducationalContent _originalEducationalContentLinkToQuiz = new Entities.QuizEducationalContent();

                        _originalEducationalContentLinkToQuiz = _context.QuizEducationalContents.Where(qec => qec.QuizID.Equals(_quizInformation.QuizID) && qec.QuizVersion.Equals(_quizInformation.Quizversion)).FirstOrDefault();

                        _updatedEducationalContentLinkToQuiz.QuizID = _quizInformation.QuizID;
                        _updatedEducationalContentLinkToQuiz.QuizVersion = _quizInformation.Quizversion;
                        _updatedEducationalContentLinkToQuiz.EducationalContentID = int.Parse(_quizInformation.RelatedSource);

                        if (_originalEducationalContentLinkToQuiz != null)
                        {
                            _context.Entry(_originalEducationalContentLinkToQuiz).CurrentValues.SetValues(_updatedEducationalContentLinkToQuiz);

                        }
                        else
                        {
                            _context.QuizEducationalContents.Add(_updatedEducationalContentLinkToQuiz);
                            _context.SaveChanges();
                        }

                    }

                    //***NOT Entity Framework (Because Only Need Questions Marked as Activo, not InActive).
                    _updated = _updated.readQuizByIDAndVersion(_updatedQuiz.QuizID.ToString(), _updatedQuiz.QuizVersion.ToString());
                   ///***********************


                        return Ok(_updated);
                    }

                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }

                return Ok("Quiz Not Found");

            }


            public string EFupdateQuizQuestion(FarmworkersWebAPI.Entities.QuizQuestion _updatedQuizQuestion)
            {
                if (_updatedQuizQuestion.QuestionID < 0)
                    return "Quiz Question ID can not be negative";

                if (_updatedQuizQuestion.QuestionID == 0)
                    return "Quiz Question ID Not Specified";

                _context.Configuration.ProxyCreationEnabled = false;

                try
                {
                    FarmworkersWebAPI.Entities.QuizQuestion original = _context.QuizQuestions.
                                                            Where(q => q.QuizID.Equals(_updatedQuizQuestion.QuizID) && q.QuizVersion.Equals(_updatedQuizQuestion.QuizVersion) && q.QuestionID.Equals(_updatedQuizQuestion.QuestionID)).FirstOrDefault();
                
                    if (original != null)
                    {
                        //Update Question Base Information
                        _context.Entry(original).CurrentValues.SetValues(_updatedQuizQuestion);
                        _context.SaveChanges();


                        //Determine if there are less answers in this quiz question update than in the previous version of the question
                        //If there are less answers in the new update, the ones left over from the previous version will be deleted
                        string _deletionResult = deleteOldLeftOverAnswers(_updatedQuizQuestion);

                        if (_deletionResult != "Answers Deleted Successfully")
                        {
                            return _deletionResult;
                        }
                        //*********

                        foreach (FarmworkersWebAPI.Entities.QuizAnswer _answer in _updatedQuizQuestion.QuizAnswers)
                        {
                        
                            string _updateResult = EFupdateQuizAnswer(_answer);

                            if (_updateResult != "Answer Updated Successfully")
                            {
                                return _updateResult;
                            }

                        }
                    
                        return "Question Updated Successfully";
                    }
                    else  //If during the update process, there is a new question that wasn't there before.
                    {
                        //Insert New Question
                        string _insertQuestionResult = EFinsertQuizQuestion(_updatedQuizQuestion);

                        if (_insertQuestionResult != "Question Inserted Successfully")
                        {
                            return _insertQuestionResult;
                        }

                        //Insert All Answers for that Question
                        foreach (FarmworkersWebAPI.Entities.QuizAnswer _answer in _updatedQuizQuestion.QuizAnswers)
                        {
                            string _insertAnswerResult = EFinsertQuizAnswer(_answer);

                            if (_insertAnswerResult != "Answer Inserted Successfully")
                            {
                                return _insertAnswerResult;
                            }
                   
                        }

                        return "Question Updated Successfully";
                    }
                
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex).ToString();
                }
            

            }

            public string EFinsertQuizQuestion(FarmworkersWebAPI.Entities.QuizQuestion _newQuizQuestion)
            {

                _context.Configuration.ProxyCreationEnabled = false;

                FarmworkersWebAPI.Entities.QuizQuestion _quizQuestionContentData = new FarmworkersWebAPI.Entities.QuizQuestion();

                try
                {
                    _quizQuestionContentData = _context.QuizQuestions.Add(_newQuizQuestion);
                    _context.SaveChanges();

                    return "Question Inserted Successfully";
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex).ToString();
                }

            }

            public string EFupdateQuizAnswer(FarmworkersWebAPI.Entities.QuizAnswer _updatedQuizAnswer)
            {
                if (_updatedQuizAnswer.AnswerID < 0)
                    return "Quiz Answer ID can not be negative";

                if (_updatedQuizAnswer.AnswerID == 0)
                    return "Quiz Answer ID Not Specified";

                _context.Configuration.ProxyCreationEnabled = false;

                try
                {
                    FarmworkersWebAPI.Entities.QuizAnswer original = _context.QuizAnswers.
                                                                        Where(q => q.QuizID.Equals(_updatedQuizAnswer.QuizID) && q.QuizVersion.Equals(_updatedQuizAnswer.QuizVersion) && q.QuestionID.Equals(_updatedQuizAnswer.QuestionID) && q.AnswerID.Equals(_updatedQuizAnswer.AnswerID)).FirstOrDefault();

                    if (original != null)
                    {
                        _context.Entry(original).CurrentValues.SetValues(_updatedQuizAnswer);
                        _context.SaveChanges();

                    
                        return "Answer Updated Successfully";
                    }
                    else  //If during the update process, there is a new answer that wasn't there before.
                    {
                            string _insertAnswerResult = EFinsertQuizAnswer(_updatedQuizAnswer);

                            if (_insertAnswerResult != "Answer Inserted Successfully")
                            {
                                return _insertAnswerResult;
                            }

                        return "Answer Updated Successfully";
                    }
                
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex).ToString();
                }

           

            }

            public string EFinsertQuizAnswer(FarmworkersWebAPI.Entities.QuizAnswer _newQuizAnswer)
            {

                _context.Configuration.ProxyCreationEnabled = false;

                FarmworkersWebAPI.Entities.QuizAnswer _quizAnswerContentData = new FarmworkersWebAPI.Entities.QuizAnswer();

                try
                {
                    _quizAnswerContentData = _context.QuizAnswers.Add(_newQuizAnswer);
                    _context.SaveChanges();

                    return "Answer Inserted Successfully";
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex).ToString();
                }

            }

            public string EFdeleteQuizQuestion(FarmworkersWebAPI.Entities.QuizQuestion _deletedQuizQuestion)
            {
                _context.Configuration.ProxyCreationEnabled = false;

                try
                {
                    _context.QuizQuestions.Attach(_deletedQuizQuestion);
                    _context.QuizQuestions.Remove(_deletedQuizQuestion);
                    _context.SaveChanges();

                    return "Question Deleted Successfully";

                }
                catch (Exception ex)
                {
                    return InternalServerError(ex).ToString();
                }

            }

            public string EFdeleteQuizAnswer(FarmworkersWebAPI.Entities.QuizAnswer _deletedQuizAnswer)
            {
                _context.Configuration.ProxyCreationEnabled = false;

                try
                {
                    _context.QuizAnswers.Attach(_deletedQuizAnswer);
                    _context.QuizAnswers.Remove(_deletedQuizAnswer);
                    _context.SaveChanges();

                    return "Answer Deleted Successfully";

                }
                catch (Exception ex)
                {
                    return InternalServerError(ex).ToString();
                }

            }

            public string deleteOldLeftOverQuestions(Entities.Quiz _updatedQuiz)
            {

                string _deletionResult = "Questions Deleted Successfully";
                try
                {

                    List<FarmworkersWebAPI.Entities.QuizQuestion> _oldQuestions =
                                                _context.QuizQuestions.
                                                 Where(q => q.QuizID.Equals(_updatedQuiz.QuizID) &&
                                                            q.QuizVersion.Equals(_updatedQuiz.QuizVersion) &&
                                                            q.IsActive.Equals("1")).ToList();

                    if (_oldQuestions.Count > _updatedQuiz.QuizQuestions.Count && _updatedQuiz.QuizQuestions.Count != 0)
                    {
                        for (int i = _updatedQuiz.QuizQuestions.Count + 1; i <= _oldQuestions.Count; i++)
                        {
                            Entities.QuizQuestion _questionToDelete = new Entities.QuizQuestion();

                            FarmworkersWebAPI.Entities.QuizQuestion original = _context.QuizQuestions.
                                            Where(q => q.QuizID.Equals(_updatedQuiz.QuizID) && 
                                                  q.QuizVersion.Equals(_updatedQuiz.QuizVersion) && 
                                                  q.QuestionID.Equals(i)).FirstOrDefault();

                            _questionToDelete.QuizID = _updatedQuiz.QuizID;
                            _questionToDelete.QuizVersion = _updatedQuiz.QuizVersion;
                            _questionToDelete.QuestionID = i;
                            _questionToDelete.Question = original.Question;
                            _questionToDelete.IsActive = "0";
                            _questionToDelete.DateModified = original.DateModified;

                            _context.Entry(original).CurrentValues.SetValues(_questionToDelete);

                            _context.SaveChanges();
                        }
                    }

                }
                catch(Exception ex)
                {
                   return InternalServerError(ex).ToString();
                }

                return _deletionResult;
            }

            public string deleteOldLeftOverAnswers(Entities.QuizQuestion _updatedQuestion)
            {

                string _deletionResult = "Answers Deleted Successfully";
                try
                {

                    List<FarmworkersWebAPI.Entities.QuizAnswer> _oldAnswers =
                                                _context.QuizAnswers.
                                                 Where(q => q.QuizID.Equals(_updatedQuestion.QuizID) &&
                                                            q.QuizVersion.Equals(_updatedQuestion.QuizVersion) &&
                                                            q.QuestionID.Equals(_updatedQuestion.QuestionID) &&
                                                            q.IsActive.Equals("1")).ToList();

                    if (_oldAnswers.Count > _updatedQuestion.QuizAnswers.Count && _updatedQuestion.QuizAnswers.Count != 0)
                    {
                        for (int i = _updatedQuestion.QuizAnswers.Count + 1; i <= _oldAnswers.Count; i++)
                        {
                            Entities.QuizAnswer _answerToDelete = new Entities.QuizAnswer();

                            FarmworkersWebAPI.Entities.QuizAnswer original = _context.QuizAnswers.
                                            Where(q => q.QuizID.Equals(_updatedQuestion.QuizID) &&
                                                  q.QuizVersion.Equals(_updatedQuestion.QuizVersion) &&
                                                  q.QuestionID.Equals(_updatedQuestion.QuestionID) &&
                                                  q.AnswerID.Equals(i)).FirstOrDefault();

                            _answerToDelete.QuizID = _updatedQuestion.QuizID;
                            _answerToDelete.QuizVersion = _updatedQuestion.QuizVersion;
                            _answerToDelete.QuestionID = _updatedQuestion.QuestionID;
                            _answerToDelete.AnswerID = i;
                            _answerToDelete.Answer = original.Answer;
                            _answerToDelete.IsCorrect = original.IsCorrect;
                            _answerToDelete.IsActive = "0";
                            _answerToDelete.DateModified = original.DateModified;

                            _context.Entry(original).CurrentValues.SetValues(_answerToDelete);

                            _context.SaveChanges();

                        }
                    }

                }
                catch (Exception ex)
                {
                    return InternalServerError(ex).ToString();
                }

                return _deletionResult;
            }


            //Utility Function
            public Entities.Quiz EFhandleIncomingQuizFormat(ViewModels.QuizForm _quiz)
            {
                Entities.Quiz _quizCorrectFormat = new Entities.Quiz();
                Entities.QuizQuestion _quizQuestionCorrectFormat = new Entities.QuizQuestion();
                Entities.QuizAnswer _quizAnswerCorrectFormat = new Entities.QuizAnswer();

                List<Entities.QuizQuestion> _quizQuestionsLISTCorrectFormat = new List<Entities.QuizQuestion>();
                List<Entities.QuizAnswer> _questionAnswersLISTCorrectFormat = new List<Entities.QuizAnswer>();

                try
                {

                    _quizCorrectFormat.QuizID = _quiz.QuizID;
                    _quizCorrectFormat.QuizVersion = _quiz.Quizversion;
                    _quizCorrectFormat.QuizName = _quiz.Name;
                    _quizCorrectFormat.QuizDescription = _quiz.QuizDescription;
                    _quizCorrectFormat.Scale = _quiz.Scale;
                    _quizCorrectFormat.IsActive = _quiz.Status;
                    _quizCorrectFormat.DateModified = _quiz.LastModifiedDate;

                    foreach (ViewModels.QuizQuestionForm _question in _quiz.QuizQuestions)
                    {
                        _quizQuestionCorrectFormat = new Entities.QuizQuestion();

                        _quizQuestionCorrectFormat.QuizID = _quiz.QuizID;
                        _quizQuestionCorrectFormat.QuizVersion = _quiz.Quizversion;
                        _quizQuestionCorrectFormat.QuestionID = _question.QuestionID;
                        _quizQuestionCorrectFormat.Question = _question.Question;
                        _quizQuestionCorrectFormat.IsActive = "1";
                        _quizQuestionCorrectFormat.DateModified = _quiz.LastModifiedDate;

                        string _correctAnswer = _question.CorrectA;

                        _questionAnswersLISTCorrectFormat = new List<Entities.QuizAnswer>();

                        foreach (ViewModels.QuizAnswerForm _answer in _question.Answers)
                        {
                            _quizAnswerCorrectFormat = new Entities.QuizAnswer();

                            _quizAnswerCorrectFormat.QuizID = _quiz.QuizID;
                            _quizAnswerCorrectFormat.QuizVersion = _quiz.Quizversion;
                            _quizAnswerCorrectFormat.QuestionID = _question.QuestionID;
                            _quizAnswerCorrectFormat.AnswerID = _answer.AnswerID;
                            _quizAnswerCorrectFormat.Answer = _answer.Answer;
                            _quizAnswerCorrectFormat.IsCorrect = _answer.AnswerID.ToString() == _question.CorrectA ? 1 : 0;
                            _quizAnswerCorrectFormat.IsActive = "1";
                            _quizAnswerCorrectFormat.DateModified = _quiz.LastModifiedDate;

                            //Add Answer to Answer List
                            _questionAnswersLISTCorrectFormat.Add(_quizAnswerCorrectFormat);

                        }

                        //Add List of Answers to a Single Question
                        _quizQuestionCorrectFormat.QuizAnswers = _questionAnswersLISTCorrectFormat;

                        //Add Complete Question with Answers, to the Question List
                        _quizQuestionsLISTCorrectFormat.Add(_quizQuestionCorrectFormat);
                    }

                    _quizCorrectFormat.QuizQuestions = _quizQuestionsLISTCorrectFormat;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return _quizCorrectFormat;
            }


        //*********************//


 
        //Quiz
        [HttpGet]
        public IEnumerable<Quiz> GetListOfQuizzesController()
        {
            List<Quiz> _listOfQuizzes = new List<Quiz>();

            Quiz _quizInstance = new Quiz();

            _listOfQuizzes = _quizInstance.readAllQuizzes();

            return _listOfQuizzes;
        }

        [HttpGet]
        public Quiz GetQuizByIDAndVersionController(string _quizID, string _quizVersion)
        {
            Quiz _quizInstance = new Quiz();

            _quizInstance = _quizInstance.readQuizByIDAndVersion(_quizID, _quizVersion);

            return _quizInstance;
        }

        [HttpGet]
        public string GetLatestQuizVersionController(string _quizID)
        {
            Quiz _quizInstance = new Quiz();

            return _quizInstance.readLatestQuizVersion(_quizID); ;
        }

        [HttpGet]
        public string GetLatestQuizIDController()
        {
            Quiz _quizInstance = new Quiz();

            string latestQuizID = _quizInstance.readLatestQuizID();

            return latestQuizID;
        }

        [HttpGet]
        public string DeleteQuiz(string _quizID, string _quizVersion)
        {
            Quiz _quizInstance = new Quiz();        

            return _quizInstance.DeleteQuiz(_quizID, _quizVersion);
        }

        [HttpPost]
        public IHttpActionResult InsertQuiz(ViewModels.QuizForm _quizInformation)
        {
            Quiz _quizReceived = new Quiz();

            try
            {
                //The incoming format coming from the Front-End is changed so it is adjusted to the Format in the data base.
                _quizReceived = handleIncomingQuizFormat(_quizInformation);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            string _insertionStatus = _quizReceived.insertQuiz(_quizReceived);

            if (_insertionStatus != "Quiz Question Inserted Successfully")
            {
                return Ok(GetQuizByIDAndVersionController(_quizReceived.QuizID.ToString(), _quizReceived.QuizVersion.ToString()));
            }
            else
            {
                return Ok(_insertionStatus);
            }
        }


        //Quiz User Attempt

        [HttpPost]
        public IHttpActionResult InsertUserQuizAttempt(QuizAttemptsUser _quizInformation)
        {
            Quiz _quizReceived = new Quiz();

            string _insertionStatus = _quizReceived.InsertUserQuizAttempt(_quizInformation);

            if (_insertionStatus != "User Quiz Attempt was stored successfully")
            {
                return Ok(_insertionStatus);
            }

            return Ok(_insertionStatus);
        }

        [HttpGet]
        public string DeleteQuizAttemptUser(string _quizAttemptID)
        {
            Quiz _quizInstance = new Quiz();

            return _quizInstance.DeleteQuizAttemptUser(_quizAttemptID);
        }

        [HttpPost]
        public IHttpActionResult UpdateUserQuizAttempt(QuizAttemptsUser _quizInformation)
        {
            Quiz _quizReceived = new Quiz();

            string _updateStatus = _quizReceived.UpdateQuizAttemptUser(_quizInformation);

            if (_updateStatus != "User Quiz Attempt was stored successfully")
            {
                return Ok(_updateStatus);
            }

            return Ok(_updateStatus);
        }

        [HttpGet]
        public IEnumerable<QuizAttemptsUser> ReadQuizAttemptUserByAnyParameter(string _parameterName, string _parameterData, string _exactMatch)
        {
            Quiz _quizInstance = new Quiz();
            DataTable _quizAttemptDataFromDataBase = new DataTable();

            _quizAttemptDataFromDataBase = _quizInstance.ReadQuizAttemptUserByAnyParameter(_parameterName, _parameterData, _exactMatch);

            QuizAttemptsUser[] _listOfQuizAttempts = new QuizAttemptsUser[_quizAttemptDataFromDataBase.Rows.Count];

            int quizAttemptCounter = 0;

            foreach (DataRow _quizAttemptRow in _quizAttemptDataFromDataBase.Rows)
            {


                QuizAttemptsUser _quizAttempt = new QuizAttemptsUser
                {
                    ID = int.Parse(_quizAttemptRow["ID"].ToString()),
                    UserID = int.Parse(_quizAttemptRow["UserID"].ToString()),
                    QuizID = int.Parse(_quizAttemptRow["QuizID"].ToString()),
                    QuizVersion = int.Parse(_quizAttemptRow["QuizVersion"].ToString()),
                    QuizScore = int.Parse(_quizAttemptRow["QuizScore"].ToString()),
                    UserAnswers = _quizAttemptRow["UserAnswers"].ToString(),
                    IsFinished = _quizAttemptRow["IsFinished"].ToString(),
                    DateTaken = _quizAttemptRow["DateTaken"].ToString(),

                };

                _listOfQuizAttempts[quizAttemptCounter] = _quizAttempt;

                quizAttemptCounter++;
            }

            return _listOfQuizAttempts;
        }

        
        //Utility Function
        public Quiz handleIncomingQuizFormat(ViewModels.QuizForm _quiz)
        {
            Quiz _quizCorrectFormat = new Quiz();
            QuizQuestion _quizQuestionCorrectFormat = new QuizQuestion();
            QuizAnswer _quizAnswerCorrectFormat = new QuizAnswer();

            List<QuizQuestion> _quizQuestionsLISTCorrectFormat = new List<QuizQuestion>();
            List<QuizAnswer> _questionAnswersLISTCorrectFormat = new List<QuizAnswer>();

            try
            {

                _quizCorrectFormat.QuizID = _quiz.QuizID;
                _quizCorrectFormat.QuizVersion = _quiz.Quizversion;
                _quizCorrectFormat.QuizName = _quiz.Name;
                _quizCorrectFormat.QuizDescription = _quiz.QuizDescription;
                _quizCorrectFormat.Scale = _quiz.Scale != null ? long.Parse(_quiz.Scale) : _quiz.QuizQuestions.Count;
                _quizCorrectFormat.IsActive = _quiz.Status;
                _quizCorrectFormat.DateModified = _quiz.LastModifiedDate.ToString();

                foreach (ViewModels.QuizQuestionForm _question in _quiz.QuizQuestions)
                {
                    _quizQuestionCorrectFormat = new QuizQuestion();

                    _quizQuestionCorrectFormat.QuizID = _quiz.QuizID;
                    _quizQuestionCorrectFormat.QuizVersion = _quiz.Quizversion;
                    _quizQuestionCorrectFormat.QuestionID = _question.QuestionID;
                    _quizQuestionCorrectFormat.Question = _question.Question;
                    _quizQuestionCorrectFormat.IsActive = "1";
                    _quizQuestionCorrectFormat.DateModified = _quiz.LastModifiedDate.ToString();

                    string _correctAnswer = _question.CorrectA;

                    _questionAnswersLISTCorrectFormat = new List<QuizAnswer>();

                    foreach (ViewModels.QuizAnswerForm _answer in _question.Answers)
                    {
                        _quizAnswerCorrectFormat = new QuizAnswer();

                        _quizAnswerCorrectFormat.QuizID = _quiz.QuizID;
                        _quizAnswerCorrectFormat.QuizVersion = _quiz.Quizversion;
                        _quizAnswerCorrectFormat.QuestionID = _question.QuestionID;
                        _quizAnswerCorrectFormat.AnswerID = _answer.AnswerID;
                        _quizAnswerCorrectFormat.Answer = _answer.Answer;
                        _quizAnswerCorrectFormat.IsCorrect = _answer.AnswerID.ToString() == _question.CorrectA ? "1" : "0";
                        _quizAnswerCorrectFormat.IsActive = "1";
                        _quizAnswerCorrectFormat.DateModified = _quiz.LastModifiedDate.ToString();

                        //Add Answer to Answer List
                        _questionAnswersLISTCorrectFormat.Add(_quizAnswerCorrectFormat);

                    }

                    //Add List of Answers to a Single Question
                    _quizQuestionCorrectFormat.QuizAnswers = _questionAnswersLISTCorrectFormat;

                    //Add Complete Question with Answers, to the Question List
                    _quizQuestionsLISTCorrectFormat.Add(_quizQuestionCorrectFormat);
                }

                _quizCorrectFormat.QuizQuestions = _quizQuestionsLISTCorrectFormat;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _quizCorrectFormat;
        }

    }
}
