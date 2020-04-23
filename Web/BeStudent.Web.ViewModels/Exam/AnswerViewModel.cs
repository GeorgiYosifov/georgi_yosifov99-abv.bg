namespace BeStudent.Web.ViewModels.Exam
{
    using BeStudent.Data.Models;
    using BeStudent.Services.Mapping;

    public class AnswerViewModel : IMapFrom<Answer>
    {
        public string Text { get; set; }

        public double Points { get; set; }

        public AnswerType Type { get; set; }
    }
}
