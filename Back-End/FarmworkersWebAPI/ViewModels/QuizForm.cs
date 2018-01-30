using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmworkersWebAPI.ViewModels
{
    public class QuizForm
    {
        public int QuizID { get; set; }
        public string Name { get; set; }
        public int Quizversion { get; set; }
        public string QuizDescription { get; set; }
        public string RelatedSource { get; set; }
        public string Scale { get; set; }
        public string Status { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public List<QuizQuestionForm> QuizQuestions { get; set; }

    }

    public class QuizQuestionForm
    {
        public int QuestionID { get; set; }
        public string Question { get; set; }
        public List<QuizAnswerForm> Answers { get; set; }
        public string CorrectA { get; set; }        
    }

    public class QuizAnswerForm
    {
        public int AnswerID { get; set; }
        public int QuestionID { get; set; }
        public string Answer { get; set; }
    }
}