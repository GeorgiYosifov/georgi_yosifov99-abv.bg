namespace BeStudent.Web.ViewModels.Exam
{
    using System.Collections.Generic;

    public class AnswersListCreateInputModel
    {
        public int OnlineTestId { get; set; }

        public string QuestionId { get; set; }

        public IList<AnswerCreateInputModel> AnswerCreateInputs { get; set; }
    }
}
