namespace BeStudent.Web.ViewModels.Exam
{
    using System.Collections.Generic;

    public class AnswersListCreateInputModel
    {
        public AnswersListCreateInputModel()
        {
            for (int i = 0; i < this.NumberOfAnswers; i++)
            {
                this.AnswerCreateInputs.Add(new AnswerCreateInputModel());
            }
        }

        public int NumberOfAnswers { get; set; }

        public int OnlineTestId { get; set; }

        public string QuestionId { get; set; }

        public IList<AnswerCreateInputModel> AnswerCreateInputs { get; set; }
    }
}
