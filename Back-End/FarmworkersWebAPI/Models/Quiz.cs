using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FarmworkersWebAPI.Controllers;

namespace FarmworkersWebAPI.Models
{
    public class Quiz
    {
        public Int64 QuizID { get; set; }
        public Int64 QuizVersion { get; set; }
        public string QuizName { get; set; }
        public string QuizDescription { get; set; }
        public Int64 Scale { get; set; }
        public string IsActive { get; set; }
        public string DateModified { get; set; }
        

        public List<QuizQuestion> QuizQuestions { get; set; }

        public IEnumerable<EducationalContent> EducationalContents { get; set; }

        public List<Quiz> readAllQuizzes()
        {
            DataTable _dataBaseResults = new DataTable();

            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            string _queryString = "Exec  ReadAllQuizzes";

            List<Quiz> _listOfQuizzes = new List<Quiz>();

            try
            {
                SqlDataAdapter _sqlAdapter = new SqlDataAdapter(_queryString, _dataSource);
                _sqlAdapter.Fill(_dataBaseResults);

            }
            catch (Exception _exceptionData)
            {
                return _listOfQuizzes;
            }

            foreach (DataRow _row in _dataBaseResults.Rows)
            {
                Quiz _newQuiz = new Quiz();
                _newQuiz.QuizID = _row["QuizID"].ToString() != ""? Int64.Parse(_row["QuizID"].ToString()):0;
                _newQuiz.QuizVersion = _row["QuizVersion"].ToString() != "" ? Int64.Parse(_row["QuizVersion"].ToString()) : 0;
                _newQuiz.QuizName = _row["QuizName"].ToString();
                _newQuiz.QuizDescription = _row["QuizDescription"].ToString();
                _newQuiz.Scale = _row["Scale"].ToString() != "" ? Int64.Parse(_row["Scale"].ToString()) : 0;
                _newQuiz.IsActive = _row["IsActive"].ToString();
                _newQuiz.DateModified = _row["DateModified"].ToString();


                if ( _newQuiz.QuizVersion != 0 && _newQuiz.QuizID != 0)
                {
                    _newQuiz.QuizQuestions = readAllQuizQuestion(_newQuiz.QuizID, _newQuiz.QuizVersion);
                    _listOfQuizzes.Add(_newQuiz);
                }

            }


            return _listOfQuizzes;

        }

        public List<QuizQuestion> readAllQuizQuestion(Int64 _quizID, Int64 _quizVersion)
        {
            DataTable _dataBaseResults = new DataTable();

            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

             String query = "ReadQuizQuestionsByID&Version";
             SqlCommand command = new SqlCommand(query, myConnection);
             command.CommandType = CommandType.StoredProcedure;

             //--Data of User Table--
             command.Parameters.AddWithValue("@QuizID", _quizID);
             command.Parameters.AddWithValue("@QuizVersion", _quizVersion);

            List<QuizQuestion> _listOfQuizQuestions = new List<QuizQuestion>();

            try
            {
                myConnection.Open();

                _dataBaseResults.Load(command.ExecuteReader());

                myConnection.Close();

            }
            catch (Exception _exceptionData)
            {
                return _listOfQuizQuestions;
            }

            foreach (DataRow _row in _dataBaseResults.Rows)
            {
                QuizQuestion _newQuizQuestion = new QuizQuestion();
                _newQuizQuestion.QuestionID = _row["QuestionID"].ToString() != "" ? Int64.Parse(_row["QuestionID"].ToString()) : 0;
                _newQuizQuestion.QuizVersion = _row["QuizVersion"].ToString() != "" ? Int64.Parse(_row["QuizVersion"].ToString()) : 0;
                _newQuizQuestion.QuizID = _row["QuizID"].ToString() != "" ? Int64.Parse(_row["QuizID"].ToString()) : 0;

                _newQuizQuestion.Question = _row["Question"].ToString();
                _newQuizQuestion.IsActive = _row["IsActive"].ToString();
                _newQuizQuestion.DateModified = _row["DateModified"].ToString();
                _newQuizQuestion.CorrectAnswer = _row["CorrectAnswer"].ToString();



                if (_newQuizQuestion.QuestionID != 0 && _newQuizQuestion.QuizVersion != 0 && _newQuizQuestion.QuizID != 0)
                {
                    _newQuizQuestion.QuizAnswers = readAllQuizQuestionAnswers(_newQuizQuestion.QuizID, _newQuizQuestion.QuizVersion, _newQuizQuestion.QuestionID);
                    _listOfQuizQuestions.Add(_newQuizQuestion);
                }

            }

            return _listOfQuizQuestions;

        }

        public List<QuizAnswer> readAllQuizQuestionAnswers(Int64 _quizID, Int64 _quizVersion, Int64 _questionID)
        {
            DataTable _dataBaseResults = new DataTable();

            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            String query = "ReadQuizAnswersByID&Version";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of User Table--
            command.Parameters.AddWithValue("@QuizID", _quizID);
            command.Parameters.AddWithValue("@QuizVersion", _quizVersion);
            command.Parameters.AddWithValue("@QuizQuestionID", _questionID);

            List<QuizAnswer> _listOfQuizQuestionAnswers = new List<QuizAnswer>();
            try
            {
                myConnection.Open();

                _dataBaseResults.Load(command.ExecuteReader());

                myConnection.Close();

            }
            catch (Exception _exceptionData)
            {
                return _listOfQuizQuestionAnswers;
            }

            foreach (DataRow _row in _dataBaseResults.Rows)
            {
                QuizAnswer _newQuizQuestionAnswer = new QuizAnswer();
                _newQuizQuestionAnswer.QuestionID = _row["QuestionID"].ToString() != "" ? Int64.Parse(_row["QuestionID"].ToString()) : 0;
                _newQuizQuestionAnswer.QuizVersion = _row["QuizVersion"].ToString() != "" ? Int64.Parse(_row["QuizVersion"].ToString()) : 0;
                _newQuizQuestionAnswer.QuizID = _row["QuizID"].ToString() != "" ? Int64.Parse(_row["QuizID"].ToString()) : 0;

                _newQuizQuestionAnswer.AnswerID = _row["AnswerID"].ToString() != "" ? Int64.Parse(_row["AnswerID"].ToString()) : 0;
                _newQuizQuestionAnswer.Answer = _row["Answer"].ToString();
                _newQuizQuestionAnswer.IsActive = _row["IsActive"].ToString();
                _newQuizQuestionAnswer.IsCorrect = _row["IsCorrect"].ToString();
                _newQuizQuestionAnswer.DateModified = _row["DateModified"].ToString();

                if(_newQuizQuestionAnswer.QuestionID != 0 && _newQuizQuestionAnswer.QuizVersion != 0 &&  _newQuizQuestionAnswer.QuizID != 0 && _newQuizQuestionAnswer.AnswerID != 0)
                    _listOfQuizQuestionAnswers.Add(_newQuizQuestionAnswer);

            }


            return _listOfQuizQuestionAnswers;

        }

        public Quiz readQuizByIDAndVersion(string _quizID, string _quizVersion)
        {
            DataTable _dataBaseResults = new DataTable();

            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            String query =  "ReadQuizByID&Version";

            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of User Table--
            command.Parameters.AddWithValue("@QuizID", _quizID);
            command.Parameters.AddWithValue("@QuizVersion", _quizVersion == "Latest" ? readLatestQuizVersion(_quizID) : _quizVersion);

            Quiz _newQuiz = new Quiz();

            EducationalContentController _newEducationalContent = new EducationalContentController();

            try
            {
                myConnection.Open();

                _dataBaseResults.Load(command.ExecuteReader());

                myConnection.Close();

            }
            catch (Exception _exceptionData)
            {
                return _newQuiz;
            }

            if (_dataBaseResults.Rows.Count > 0)
            {

                _newQuiz.QuizID = _dataBaseResults.Rows[0]["QuizID"].ToString() != "" ? Int64.Parse(_dataBaseResults.Rows[0]["QuizID"].ToString()) : 0;
                _newQuiz.QuizVersion = _dataBaseResults.Rows[0]["QuizVersion"].ToString() != "" ? Int64.Parse(_dataBaseResults.Rows[0]["QuizVersion"].ToString()) : 0;
                _newQuiz.QuizName = _dataBaseResults.Rows[0]["QuizName"].ToString();
                _newQuiz.QuizDescription = _dataBaseResults.Rows[0]["QuizDescription"].ToString();
                _newQuiz.Scale = _dataBaseResults.Rows[0]["Scale"].ToString() != "" ? Int64.Parse(_dataBaseResults.Rows[0]["Scale"].ToString()) : 0;
                _newQuiz.IsActive = _dataBaseResults.Rows[0]["IsActive"].ToString();
                _newQuiz.DateModified = _dataBaseResults.Rows[0]["DateModified"].ToString();
                _newQuiz.EducationalContents = _newEducationalContent.GetListOfEducationalContentsbyAnyParameterController("IDEducationalContent", _dataBaseResults.Rows[0]["EducationalContentLinked"].ToString(), "Yes");

                if (_newQuiz.QuizVersion != 0 && _newQuiz.QuizID != 0)
                {
                    _newQuiz.QuizQuestions = readAllQuizQuestion(_newQuiz.QuizID, _newQuiz.QuizVersion);
                }

            }

            return _newQuiz;
        }

        public string readLatestQuizVersion(string _quizID)
        {
            DataTable _dataBaseResults = new DataTable();

            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            String query = "ReadLatestQuizVersion";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of User Table--
            command.Parameters.AddWithValue("@QuizID", _quizID);

            string _quizVersion = "0";

            try
            {
                myConnection.Open();

                _dataBaseResults.Load(command.ExecuteReader());

                myConnection.Close();

            }
            catch (Exception _exceptionData)
            {
                return "0";
            }

            if(_dataBaseResults.Rows.Count > 0)
            {
                _quizVersion = _dataBaseResults.Rows[0]["QuizVersion"].ToString();
            }

            return _quizVersion;
        }

        public string readLatestQuizID()
        {
            DataTable _dataBaseResults = new DataTable();

            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            String query = "ReadLatestQuizID";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;


            string _quizIDString = "0";

            try
            {
                myConnection.Open();

                _dataBaseResults.Load(command.ExecuteReader());

                myConnection.Close();

            }
            catch (Exception _exceptionData)
            {
                return "0";
            }

            if (_dataBaseResults.Rows.Count > 0)
            {
                Int64 _quizID = Int64.Parse(_dataBaseResults.Rows[0]["QuizID"].ToString()) + 1;
                _quizIDString = _quizID.ToString();
            }
            else
            {
                _quizIDString = "1";
            }

            return _quizIDString;
        }

        public string insertQuiz(Quiz _newQuiz)
        {

            Quiz _quizExistCheck = readQuizByIDAndVersion(_newQuiz.QuizID.ToString(), _newQuiz.QuizVersion.ToString());

            if (_quizExistCheck.QuizName != null)
            {
                return "Error - A Quiz with that ID and Version already exists";
            }

            DataTable _dataBaseResults = new DataTable();

            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            String query = "InsertQuiz";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            if(readLatestQuizID() == "0")
                return "An error Ocurred in the Data Base";

           if( _newQuiz.QuizID < 0)
                return "Quiz ID can not be negative";

            //--Data of User Table--
            command.Parameters.AddWithValue("@QuizID",  _newQuiz.QuizID == 0? readLatestQuizID(): _newQuiz.QuizID.ToString());
            command.Parameters.AddWithValue("@QuizVersion", _newQuiz.QuizVersion == 0 ? readLatestQuizVersion(_newQuiz.QuizID.ToString()) == "0"? "1"  : readLatestQuizVersion(_newQuiz.QuizID.ToString()): _newQuiz.QuizVersion.ToString());
            command.Parameters.AddWithValue("@QuizName", _newQuiz.QuizName == null? "Name Undefined": _newQuiz.QuizName);
            command.Parameters.AddWithValue("@QuizDescription", _newQuiz.QuizDescription == null ? "Description Undefined" : _newQuiz.QuizDescription);
            command.Parameters.AddWithValue("@Scale", _newQuiz.Scale == 0 ? "Scale Undefined" : _newQuiz.Scale.ToString());
            command.Parameters.AddWithValue("@IsActive", _newQuiz.IsActive);
            command.Parameters.AddWithValue("@DateModified", DateTime.Now.ToShortDateString());


            List<QuizQuestion> _listOfQuizQuestions = new List<QuizQuestion>();

            try
            {
                myConnection.Open();

                command.ExecuteNonQuery();

                myConnection.Close();

            }
            catch (Exception _exceptionData)
            {
                return "An error Ocurred in the Data Base";
            }

            //Insert Questions

            Int64 _currentQuizID = _newQuiz.QuizID == 0 ? Int64.Parse(readLatestQuizID()) : Int64.Parse(_newQuiz.QuizID.ToString());
            Int64 _currentQuizVersion = Int64.Parse(readLatestQuizVersion(_currentQuizID.ToString()));


            foreach (QuizQuestion _question in _newQuiz.QuizQuestions)
            {
                _question.QuizID = _currentQuizID;
                _question.QuizVersion = _currentQuizVersion;

                if(insertQuizQuestion(_question) != "Quiz Question Inserted Successfully")
                    return "An error Ocurred - Quiz Base Information was Inserted - But some quiz data may have not been inserted properly";

            }

            return "Quiz Inserted Successfully";

        }



        public string insertQuizQuestion(QuizQuestion _newQuizQuestion)
        {
            DataTable _dataBaseResults = new DataTable();

            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            String query = "InsertQuizQuestion";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            if (readLatestQuizID() == "0")
                return "An error Ocurred in the Data Base";

            if (_newQuizQuestion.QuestionID <= 0)
                return "Question ID can not be negative or Zero";

            //--Data of QuizQuestion Table--
            command.Parameters.AddWithValue("@QuizID", _newQuizQuestion.QuizID == 0 ? readLatestQuizID() : _newQuizQuestion.QuizID.ToString());
            command.Parameters.AddWithValue("@QuizVersion", _newQuizQuestion.QuizVersion == 0 ? readLatestQuizVersion(_newQuizQuestion.QuizID.ToString()) == "0" ? "1" : readLatestQuizVersion(_newQuizQuestion.QuizID.ToString()) : _newQuizQuestion.QuizVersion.ToString());

            command.Parameters.AddWithValue("@QuestionID", _newQuizQuestion.QuestionID);
            command.Parameters.AddWithValue("@Question", _newQuizQuestion.Question == null || _newQuizQuestion.Question == ""? "Question Undefined": _newQuizQuestion.Question);

            command.Parameters.AddWithValue("@IsActive", _newQuizQuestion.IsActive);
            command.Parameters.AddWithValue("@DateModified", DateTime.Now.ToShortDateString());


            List<QuizQuestion> _listOfQuizQuestions = new List<QuizQuestion>();

            try
            {
                myConnection.Open();

                command.ExecuteNonQuery();

                myConnection.Close();

            }
            catch (Exception _exceptionData)
            {
                return "An error Ocurred in the Data Base";
            }

            //Insert Answers
            Int64 _currentQuizID = Int64.Parse(readLatestQuizID()) - 1;
            Int64 _currentQuizVersion = Int64.Parse(readLatestQuizVersion(_currentQuizID.ToString()));


            foreach (QuizAnswer _answer in _newQuizQuestion.QuizAnswers)
            {
                _answer.QuizID = _currentQuizID;
                _answer.QuizVersion = _currentQuizVersion;
                _answer.QuestionID = _newQuizQuestion.QuestionID;

                if (insertQuizAnswer(_answer) != "Quiz Answer Inserted Successfully")
                    return "An error Ocurred - Quiz Base Information was Inserted - But some quiz data may have not been inserted properly";

            }

            return "Quiz Question Inserted Successfully";
        }

        public string insertQuizAnswer(QuizAnswer _newQuizAnswer)
        {
            DataTable _dataBaseResults = new DataTable();

            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            String query = "InsertQuizAnswer";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            if (readLatestQuizID() == "0")
                return "An error Ocurred in the Data Base";

            if (_newQuizAnswer.AnswerID <= 0)
                return "Question ID can not be negative or Zero";

            //--Data of QuizQuestion Table--
            command.Parameters.AddWithValue("@QuizID", _newQuizAnswer.QuizID == 0 ? readLatestQuizID() : _newQuizAnswer.QuizID.ToString());
            command.Parameters.AddWithValue("@QuizVersion", _newQuizAnswer.QuizVersion == 0 ? readLatestQuizVersion(_newQuizAnswer.QuizID.ToString()) == "0" ? "1" : readLatestQuizVersion(_newQuizAnswer.QuizID.ToString()) : _newQuizAnswer.QuizVersion.ToString());

            command.Parameters.AddWithValue("@QuestionID", _newQuizAnswer.QuestionID);
            command.Parameters.AddWithValue("@AnswerID", _newQuizAnswer.AnswerID);
            command.Parameters.AddWithValue("@Answer", _newQuizAnswer.Answer == null || _newQuizAnswer.Answer == "" ? "Answer Undefined" : _newQuizAnswer.Answer);

            command.Parameters.AddWithValue("@IsCorrect", _newQuizAnswer.IsCorrect);
            command.Parameters.AddWithValue("@IsActive", _newQuizAnswer.IsActive);
            command.Parameters.AddWithValue("@DateModified", DateTime.Now.ToShortDateString());


            List<QuizQuestion> _listOfQuizQuestions = new List<QuizQuestion>();

            try
            {
                myConnection.Open();

                command.ExecuteNonQuery();

                myConnection.Close();

            }
            catch (Exception _exceptionData)
            {
                return "An error Ocurred in the Data Base";
            }

            //Insert Answers

            return "Quiz Answer Inserted Successfully";
        }
    
        public string DeleteQuiz(string _quizID, string _quizVersion)
        {
            DataTable _dataBaseResults = new DataTable();

            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            String query = "DeleteQuiz";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of User Table--
            command.Parameters.AddWithValue("@QuizID", _quizID);
            command.Parameters.AddWithValue("@QuizVersion", _quizVersion);


            try
            {
                myConnection.Open();

                command.ExecuteNonQuery();

                myConnection.Close();

            }
            catch (Exception _exceptionData)
            {
                return "Quiz Could Not be Deleted";
            }


            return "Quiz Deleted Successfully";
        }


        public string InsertUserQuizAttempt(QuizAttemptsUser _userQuizAttemptInformation)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);


            String query = "InsertQuizAttemptUser";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Table User Quiz Attempt Data-
            command.Parameters.AddWithValue("@UserID", _userQuizAttemptInformation.UserID);
            command.Parameters.AddWithValue("@QuizID", _userQuizAttemptInformation.QuizID);
            command.Parameters.AddWithValue("@QuizVersion", _userQuizAttemptInformation.QuizVersion);
            command.Parameters.AddWithValue("@QuizScore", _userQuizAttemptInformation.QuizScore);
            command.Parameters.AddWithValue("@UserAnswers", _userQuizAttemptInformation.UserAnswers);
            command.Parameters.AddWithValue("@UserAnswers", _userQuizAttemptInformation.UserAnswers);
            command.Parameters.AddWithValue("@IsFinished", _userQuizAttemptInformation.IsFinished);
            command.Parameters.AddWithValue("@DateTaken", _userQuizAttemptInformation.DateTaken);

            try
            {
                myConnection.Open();

                command.ExecuteNonQuery();

                myConnection.Close();
            }
            catch (Exception exception)
            {
                return "User Quiz Attempt was not stored in the server.";
            }

            return "User Quiz Attempt was stored successfully";
        }

        public string UpdateQuizAttemptUser(QuizAttemptsUser _userQuizAttemptInformation)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);


            String query = "UpdateQuizAttemptUser";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Table User Quiz Attempt Data-
            command.Parameters.AddWithValue("@ID", _userQuizAttemptInformation.UserID);
            command.Parameters.AddWithValue("@UserID", _userQuizAttemptInformation.UserID);
            command.Parameters.AddWithValue("@QuizID", _userQuizAttemptInformation.QuizID);
            command.Parameters.AddWithValue("@QuizVersion", _userQuizAttemptInformation.QuizVersion);
            command.Parameters.AddWithValue("@QuizScore", _userQuizAttemptInformation.QuizScore);
            command.Parameters.AddWithValue("@UserAnswers", _userQuizAttemptInformation.UserAnswers);
            command.Parameters.AddWithValue("@UserAnswers", _userQuizAttemptInformation.UserAnswers);
            command.Parameters.AddWithValue("@IsFinished", _userQuizAttemptInformation.IsFinished);
            command.Parameters.AddWithValue("@DateTaken", _userQuizAttemptInformation.DateTaken);

            try
            {
                myConnection.Open();

                command.ExecuteNonQuery();

                myConnection.Close();
            }
            catch (Exception exception)
            {
                return "User Quiz Attempt was not stored in the server.";
            }

            return "User Quiz Attempt was stored successfully";
        }

        public string DeleteQuizAttemptUser(string _quizAttemptID)
        {
            DataTable _dataBaseResults = new DataTable();

            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            String query = "DeleteQuizAttemptUser";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of User Table--
            command.Parameters.AddWithValue("@ID", _quizAttemptID);


            try
            {
                myConnection.Open();

                command.ExecuteNonQuery();

                myConnection.Close();

            }
            catch (Exception _exceptionData)
            {
                return "Quiz Attempt Could Not be Deleted";
            }


            return "Quiz Attempt Deleted Successfully";
        }

        public DataTable ReadQuizAttemptUserByAnyParameter(string _parameterName, string _parameterData, string _exactMatch)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            DataTable _educationalContentData = new DataTable();

            String query = "ReadQuizAttemptUserByAnyParameter";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of User Table--
            command.Parameters.AddWithValue("@ParameterName", _parameterName);
            command.Parameters.AddWithValue("@ParamenterData", _parameterData);
            command.Parameters.AddWithValue("@ExactMatch", _exactMatch == null ? "No" : _exactMatch);

            try
            {
                myConnection.Open();

                _educationalContentData.Load(command.ExecuteReader());

                myConnection.Close();
            }
            catch (Exception exception)
            {
                return _educationalContentData;
            }

            return _educationalContentData;


        }
    }

    public class QuizQuestion
    {
        public Int64 QuestionID { get; set; }
        public Int64 QuizID { get; set; }
        public Int64 QuizVersion { get; set; }
        public string Question { get; set; }
        public string IsActive { get; set; }
        public string DateModified { get; set; }
        public string CorrectAnswer { get; set; }
        public List<QuizAnswer> QuizAnswers { get; set; }
    }

    public class QuizAnswer
    {
        public Int64 AnswerID { get; set; }
        public Int64 QuestionID { get; set; }
        public Int64 QuizVersion { get; set; }
        public Int64 QuizID { get; set; }
        public string Answer { get; set; }
        public string IsCorrect { get; set; }
        public string IsActive { get; set; }
        public string DateModified { get; set; }
    }

    public class QuizAttemptsUser
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int QuizID { get; set; }
        public int QuizVersion { get; set; }
        public int QuizScore { get; set; }
        public string UserAnswers { get; set; }
        public string IsFinished { get; set; }
        public string DateTaken { get; set; }
    }


}